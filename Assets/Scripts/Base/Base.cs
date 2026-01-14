using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private DropPoint _dropPoint;
    [SerializeField] private ScannerResources _scannerResources;
    [SerializeField] private float _assignInterval = 0.1f;
    [SerializeField] private List<Bot> _bots = new List<Bot>();
    [SerializeField] private ResourceStorage _resourceStorage;

    private Coroutine _assignRoutine;

    public event Action<Base, Vector3> ExpansionRequested;

    public int CurrentCountBots => _bots.Count;

    public void Init(ResourceStorage resourceStorage)
    {
        _bots.Clear();

        _resourceStorage = resourceStorage;
        _scannerResources.Init(_resourceStorage);
    }

    private void Start()
    {
        foreach (Bot bot in _bots)
        {
            bot.SetDropPoint(_dropPoint);
        }
    }

    private void OnEnable()
    {
        _assignRoutine = StartCoroutine(AssignRoutine());

        _dropPoint.ResourceDelivered += OnResourceDelivered;
    }

    private void OnDisable()
    {
        if (_assignRoutine != null)
        {
            StopCoroutine(_assignRoutine);
        }

        _dropPoint.ResourceDelivered -= OnResourceDelivered;
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
        freeBot = null;

        for (int index = 0; index < _bots.Count; index++)
        {
            Bot bot = _bots[index];

            if (bot.IsBusy == false)
            {
                _bots.RemoveAt(index);
                freeBot = bot;

                return true;
            }
        }

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

        foreach (Bot bot in _bots)
        {
            if (bot.IsBusy == true)
            {
                continue;
            }

            Resource resource;

            if (_scannerResources.TryGetResource(out resource) == true)
            {
                bot.TrySetTarget(resource);
            } 
            else
            {
                break;
            }
        }
    }

    private void OnResourceDelivered(Resource resource)
    {
        _resourceStorage.Unreserve(resource);
    }
}
