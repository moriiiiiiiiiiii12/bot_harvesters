using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Transform _dropPoint;
    [SerializeField] private Transform _storageParent;
    [SerializeField] private ListResources _listResources;
    [SerializeField] private List<Bot> bots;
    [SerializeField] private float _assignInterval = 0.1f;

    private Coroutine _assignRoutine;

    public Transform DropPoint => _dropPoint;
    public Action ResourceReceived;

    private void Start()
    {
        foreach(Bot bot in bots)
        {
            bot.SetBase(this);
        }
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
        if (bots.Count == 0)
            return;

        for (int i = 0; i < bots.Count; i++)
        {
            Bot bot = bots[i];

            if (bot == null || bot.Busy)
                continue;

            if (_listResources.TryGetResource(out Resource resource))
            {
                bot.SetTarget(resource.transform, resource.UUID);
            }
            else
            {
                break;
            }
        }
    }

    public void Receive(Resource resource)
    {
        resource.Reset();

        ResourceReceived?.Invoke();
    }
}
