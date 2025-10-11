using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private Movement _movement;           
    [SerializeField] private ResourceCapture _resourceCapture;
    [SerializeField] private Base _base;

    private Transform _targetTransform;

    public bool Busy { get; private set; } = false;

    private void OnEnable()
    {
        _resourceCapture.TakeObject += OnTookResource;
    }

    private void OnDisable()
    {
        _resourceCapture.TakeObject -= OnTookResource;
    }

    public void SetTarget(Transform targetResources, string UUID)
    {
        if (Busy)
            return;

        _targetTransform = targetResources;
        _resourceCapture.SetResourceUUID(UUID);

        Busy = true;
        _movement.SetTarget(_targetTransform);
    }
    
    public void SetBase(Base @base)
    {
        _base = @base;
    }

    private void OnTookResource(Resource resource)
    {
        _movement.SetTarget(_base.DropPoint);
        _movement.ReachTarget = null;
        _movement.ReachTarget += () => GiveAway(resource);
    }

    private void GiveAway(Resource resource)
    {
        _resourceCapture.Release();
        _base.Receive(resource);
        Reset();

    }

    public void Reset()
    {
        _targetTransform = null;
        Busy = false;
        _movement.ReachTarget = null;
        _movement.Reset();
    }
}
