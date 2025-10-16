using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private DropPoint _dropPoint;
    [SerializeField] private Transform _storageParent;
    [SerializeField] private ScannerResources _scannerResources;
    [SerializeField] private List<Bot> _bots;
    [SerializeField] private Flag _flag;
    [SerializeField] private float _assignInterval = 0.1f;
    [SerializeField] private BaseFactory _baseFactory;
    [SerializeField] private BotFactory _botFactory;

    private Coroutine _assignRoutine;

    public void Init()
    {
        _bots = new List<Bot>();
    }

    private void Start()
    {
        foreach (Bot bot in _bots)
        {
            if (bot == null)
                continue;

            bot.SetDropPoint(_dropPoint);
        }

        _flag.Unset();

        _baseFactory.enabled = false;
    }

    private void OnEnable()
    {
        _assignRoutine = StartCoroutine(AssignRoutine());
    }

    private void OnDisable()
    {
        if (_assignRoutine != null)
            StopCoroutine(_assignRoutine);
    }

    public void SetFlag(Vector3 position)
    {
        _flag.Set(position);

        _baseFactory.BaseCreate += UnsetFlag;
        _baseFactory.Produce();
     
        _baseFactory.enabled = true;
        _botFactory.enabled = false;
    }

    public void UnsetFlag(Base @base)
    {
        _flag.Unset();
     
        _baseFactory.BaseCreate -= UnsetFlag;

        _baseFactory.enabled = false;
        _botFactory.enabled = true;

        StartCoroutine(GiveBotWhenAvailable(@base));
    }

    public void AddBot(Bot bot)
    {
        Debug.Log(gameObject.name + " бот добавлен");

        _bots.Add(bot);

        bot.Reset();
        bot.SetDropPoint(_dropPoint);
    }
    
    private IEnumerator GiveBotWhenAvailable(Base receivingBase)
    {
        Bot availableBot = null;

        yield return new WaitUntil(() => TryTakeFreeBot(out availableBot));

        if (receivingBase != null && availableBot != null)
        {
            receivingBase.AddBot(availableBot);
        }
    }

    private bool TryTakeFreeBot(out Bot freeBot)
    {
        for (int index = 0; index < _bots.Count; index++)
        {
            Bot candidateBot = _bots[index];
            if (candidateBot != null && candidateBot.IsBusy == false)
            {
                _bots.RemoveAt(index);
                freeBot = candidateBot;
                return true;
            }
        }

        freeBot = null;
        return false;
    }

    private IEnumerator AssignRoutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_assignInterval);

        while (enabled)
        {
            AssignTasksIfNeeded();
            
            yield return waitForSeconds;
        }
    }

    private void AssignTasksIfNeeded()
    {
        if (_bots.Count == 0)
            return;

        for (int i = 0; i < _bots.Count; i++)
        {
            Bot bot = _bots[i];

            if (bot == null || bot.IsBusy)
                continue;

            if (_scannerResources.TryGetResource(out Resource resource))
            {
                bot.SetTarget(resource.transform, resource.Id);
            }
            else
            {
                break;
            }
        }
    }
}
