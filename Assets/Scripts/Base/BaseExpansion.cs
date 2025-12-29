using System.Collections;
using UnityEngine;

public class BaseExpansion : MonoBehaviour
{
    [SerializeField] private Base _sourceBase;
    [SerializeField] private BotFactory _botFactory;
    [SerializeField] private BaseFactory _baseFactory;
    [SerializeField] private FlagPlacer _flagPlacer;

    private Coroutine _transferRoutine;
    private bool _isExpansionInProgress;

    private void Awake()
    {
        _flagPlacer.Unset();

        _baseFactory.enabled = false;
        _botFactory.enabled = true;
    }

    private void OnEnable()
    {
        _sourceBase.ExpansionRequested += OnExpansionRequested;
        _botFactory.BotCreate += OnBotCreated;
        _baseFactory.BaseCreate += OnBaseCreated;
    }

    private void OnDisable()
    {
        _sourceBase.ExpansionRequested -= OnExpansionRequested;
        _botFactory.BotCreate -= OnBotCreated;
        _baseFactory.BaseCreate -= OnBaseCreated;

        if (_transferRoutine != null)
        {
            StopCoroutine(_transferRoutine);
        }
    }

    private void OnBotCreated(Bot bot)
    {
        _sourceBase.AddBot(bot);
    }

    private void OnExpansionRequested(Base baseSender, Vector3 position)
    {
        if (_isExpansionInProgress == true)
        {
            return;
        }

        _isExpansionInProgress = true;

        _flagPlacer.Set(position);
        _baseFactory.SetSpawnPosition(position);

        _botFactory.enabled = false;
        _baseFactory.enabled = true;

        _baseFactory.Produce();
    }

    private void OnBaseCreated(Base createdBase)
    {
        if (_isExpansionInProgress == false)
        {
            return;
        }

        _flagPlacer.Unset();
        _baseFactory.ClearSpawnPosition();

        _baseFactory.enabled = false;
        _botFactory.enabled = true;

        if (_transferRoutine != null)
        {
            StopCoroutine(_transferRoutine);
        }

        _transferRoutine = StartCoroutine(TransferBotWhenAvailable(createdBase));

        _isExpansionInProgress = false;
    }

    private IEnumerator TransferBotWhenAvailable(Base receivingBase)
    {
        Bot availableBot = null;

        while (_sourceBase.TryTakeFreeBot(out availableBot) == false)
        {
            yield return null;
        }

        if (receivingBase != null && availableBot != null)
        {
            receivingBase.AddBot(availableBot);
        }
    }
}
