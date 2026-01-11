using System;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private Mover _movement;
    [SerializeField] private ResourceCapture _resourceCapture;
    [SerializeField] private DropPoint _dropPoint;

    private Resource _reservedResource;
    private Resource _carriedResource;

    private bool _isMovingToDropPoint;

    private bool _isRelocating;
    private Transform _relocationTarget;

    public int ReservationId { get; private set; }
    public bool IsBusy { get; private set; }

    public event Action<Bot> RelocationReached;

    private void Awake()
    {
        ReservationId = GetInstanceID();
    }

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

    public bool TryRelocateTo(Transform target)
    {
        if (IsBusy == true)
        {
            return false;
        }

        _reservedResource = null;
        _carriedResource = null;

        _isMovingToDropPoint = false;

        _isRelocating = true;
        _relocationTarget = target;

        IsBusy = true;
        _movement.MoveTo(_relocationTarget);

        return true;
    }

    public bool TrySetTarget(Resource resource)
    {
        if (IsBusy == true)
        {
            return false;
        }

        _reservedResource = resource;
        _carriedResource = null;

        _isRelocating = false;
        _relocationTarget = null;

        _isMovingToDropPoint = false;

        IsBusy = true;
        _movement.MoveTo(resource.transform);

        return true;
    }

    private void OnReachTarget()
    {
        if (_isRelocating == true)
        {
            _isRelocating = false;
            _relocationTarget = null;

            IsBusy = false;

            if (RelocationReached != null)
            {
                RelocationReached.Invoke(this);
            }

            return;
        }

        if (_isMovingToDropPoint == false)
        {
            bool pickedUp = _resourceCapture.TryPickUp(_reservedResource);

            if (pickedUp == false)
            {
                Reset();
            }

            return;
        }

        if (_carriedResource == null)
        {
            Reset();
            return;
        }

        GiveAway(_carriedResource);
    }

    private void OnTookResource(Resource resource)
    {
        _carriedResource = resource;

        _isMovingToDropPoint = true;
        _movement.MoveTo(_dropPoint.transform);
    }

    private void GiveAway(Resource resource)
    {
        _resourceCapture.Release();
        _dropPoint.Receive(resource);
        Reset();
    }

    public void Reset()
    {
        _resourceCapture.Release();

        _reservedResource = null;
        _carriedResource = null;

        _isRelocating = false;
        _relocationTarget = null;

        _isMovingToDropPoint = false;

        IsBusy = false;
        _movement.Reset();
    }
}
