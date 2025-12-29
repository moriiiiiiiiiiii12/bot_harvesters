using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    [SerializeField] private ResourceCapture _resourceCapture;
    [SerializeField] private DropPoint _dropPoint;

    private Transform _targetTransform;
    private Resource _tempResource;

    private bool _isMovingToDropPoint = false;

    public bool IsBusy { get; private set; } = false;

    private void OnEnable()
    {
        _resourceCapture.TakeObject += OnTookResource;
        _movement.ReachTarget += OnReachTarget;
    }

    private void OnDisable()
    {
        _resourceCapture.TakeObject -= OnTookResource;
        _movement.ReachTarget -= OnReachTarget;
    }

    public void SetDropPoint(DropPoint dropPoint)
    {
        _dropPoint = dropPoint;
    }

    public void SetTarget(Transform targetResources, int resourceId)
    {
        if (IsBusy == true)
        {
            return;
        }

        _resourceCapture.SetResourceId(resourceId);
        _targetTransform = targetResources;

        _isMovingToDropPoint = false;

        IsBusy = true;
        _movement.SetTarget(_targetTransform);
    }

    private void OnTookResource(Resource resource)
    {
        _tempResource = resource;

        _isMovingToDropPoint = true;
        _movement.SetTarget(_dropPoint.transform);
    }

    private void OnReachTarget()
    {
        if (_isMovingToDropPoint == false)
        {
            return;
        }

        if (_tempResource == null)
        {
            Reset();
            return;
        }

        GiveAway(_tempResource);
    }

    private void GiveAway(Resource resource)
    {
        _resourceCapture.Release();
        _dropPoint.Receive(resource);
        Reset();
    }

    public void Reset()
    {
        _tempResource = null;
        _targetTransform = null;

        _isMovingToDropPoint = false;

        IsBusy = false;
        _movement.Reset();
    }
}
