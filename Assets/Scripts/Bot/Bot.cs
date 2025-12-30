using System.Collections;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    [SerializeField] private ResourceCapture _resourceCapture;
    [SerializeField] private DropPoint _dropPoint;

    private Resource _reservedResource;
    private Resource _carriedResource;

    private bool _isMovingToDropPoint;

    private Coroutine _waitForCaptureRoutine;
    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

    public int ReservationId { get; private set; }
    public bool IsBusy { get; private set; }

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

        if (_waitForCaptureRoutine != null)
        {
            StopCoroutine(_waitForCaptureRoutine);
            _waitForCaptureRoutine = null;
        }
    }

    public void SetDropPoint(DropPoint dropPoint)
    {
        _dropPoint = dropPoint;
    }

    public bool TrySetTarget(Resource resource)
    {
        if (IsBusy == true)
        {
            return false;
        }

        _reservedResource = resource;
        _resourceCapture.SetTarget(resource, ReservationId);

        _isMovingToDropPoint = false;

        IsBusy = true;
        _movement.SetTarget(resource.transform);

        return true;
    }

    private void OnTookResource(Resource resource)
    {
        if (_waitForCaptureRoutine != null)
        {
            StopCoroutine(_waitForCaptureRoutine);
            _waitForCaptureRoutine = null;
        }

        _carriedResource = resource;

        _isMovingToDropPoint = true;
        _movement.SetTarget(_dropPoint.transform);
    }

    private void OnReachTarget()
    {
        if (_isMovingToDropPoint == false)
        {
            if (_waitForCaptureRoutine != null)
            {
                StopCoroutine(_waitForCaptureRoutine);
            }

            _waitForCaptureRoutine = StartCoroutine(WaitForCaptureRoutine());
            return;
        }

        if (_carriedResource == null)
        {
            Reset();
            return;
        }

        GiveAway(_carriedResource);
    }

    private IEnumerator WaitForCaptureRoutine()
    {
        yield return _waitForFixedUpdate;

        _waitForCaptureRoutine = null;

        if (_isMovingToDropPoint == false)
        {
            Reset();
        }
    }

    private void GiveAway(Resource resource)
    {
        _resourceCapture.Release();
        _dropPoint.Receive(resource);
        Reset();
    }

    public void Reset()
    {
        if (_waitForCaptureRoutine != null)
        {
            StopCoroutine(_waitForCaptureRoutine);
            _waitForCaptureRoutine = null;
        }

        if (_carriedResource == null && _reservedResource != null)
        {
            _reservedResource.CancelReservation(ReservationId);
        }

        _resourceCapture.Release();

        _reservedResource = null;
        _carriedResource = null;

        _isMovingToDropPoint = false;

        IsBusy = false;
        _movement.Reset();
    }
}
