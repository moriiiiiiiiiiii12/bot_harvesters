using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private DropPoint _dropPoint;
    [SerializeField] private ScannerResources _scannerResources;
    [SerializeField] private float _assignInterval = 0.1f;
    [SerializeField] private List<Bot> _bots = new List<Bot>();

    private Coroutine _assignRoutine;

    public event Action<Base, Vector3> ExpansionRequested;

    public void Init()
    {
        _bots.Clear();
    }

    private void Start()
    {
        for (int index = 0; index < _bots.Count; index++)
        {
            Bot bot = _bots[index];

            if (bot == null)
            {
                continue;
            }

            bot.SetDropPoint(_dropPoint);
        }
    }

    private void OnEnable()
    {
        _assignRoutine = StartCoroutine(AssignRoutine());
    }

    private void OnDisable()
    {
        if (_assignRoutine != null)
        {
            StopCoroutine(_assignRoutine);
        }
    }

    public void RequestExpansion(Vector3 position)
    {
        if (_bots.Count > 1 == false)
        {
            return;
        }

        if (ExpansionRequested != null)
        {
            ExpansionRequested.Invoke(this, position);
        }
    }

    public void AddBot(Bot bot)
    {
        if (_bots.Contains(bot) == true)
        {
            return;
        }

        _bots.Add(bot);

        bot.Reset();
        bot.SetDropPoint(_dropPoint);
    }

    public bool TryTakeFreeBot(out Bot freeBot)
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

        while (enabled == true)
        {
            AssignTasksIfNeeded();
            yield return waitForSeconds;
        }
    }

    private void AssignTasksIfNeeded()
    {
        if (_bots.Count == 0)
        {
            return;
        }

        for (int index = 0; index < _bots.Count; index++)
        {
            Bot bot = _bots[index];

            if (bot == null || bot.IsBusy == true)
            {
                continue;
            }

            Resource resource;

            if (_scannerResources.TryGetResource(bot.ReservationId, out resource) == true)
            {
                bot.TrySetTarget(resource);
            } 
            else
            {
                break;
            }
        }
    }
}
