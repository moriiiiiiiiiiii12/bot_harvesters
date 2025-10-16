using UnityEngine;


public class Bot : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    [SerializeField] private ResourceCapture _resourceCapture;
    [SerializeField] private DropPoint _dropPoint;

    private Transform _targetTransform;
    private Resource _tempResource;

    public bool IsBusy { get; private set; } = false;

    private void OnEnable()
    {
        _resourceCapture.TakeObject += OnTookResource;
    }

    private void OnDisable()
    {
        _resourceCapture.TakeObject -= OnTookResource;
    }

    public void SetDropPoint(DropPoint dropPoint)
    {
        _dropPoint = dropPoint;
    }

    public void SetTarget(Transform targetResources, int Id)
    {
        if (IsBusy)
            return;

        _resourceCapture.SetResourceId(Id);
        _targetTransform = targetResources;

        IsBusy = true;
        _movement.SetTarget(_targetTransform);
    }

    private void OnTookResource(Resource resource)
    {
        _tempResource = resource;

        _movement.SetTarget(_dropPoint.transform);

        _movement.ReachTarget += OnReachTarget;
    }

    private void OnReachTarget()
    {
        if (_tempResource == null) return; 
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
        IsBusy = false;
        _movement.ReachTarget -= OnReachTarget;
        _movement.Reset();
    }
}
