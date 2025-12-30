using System;
using UnityEngine;

public class ResourceCapture : MonoBehaviour
{
    [SerializeField] private Transform _holdPoint;

    private Resource _expectedResource;
    private Resource _carriedResource;
    private int _reservationId;

    public event Action<Resource> TakeObject;

    public void SetTarget(Resource resource, int reservationId)
    {
        _expectedResource = resource;
        _reservationId = reservationId;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_carriedResource != null)
        {
            return;
        }

        Resource resource;

        if (other.TryGetComponent(out resource) == false)
        {
            return;
        }

        if (resource != _expectedResource)
        {
            return;
        }

        if (resource.TryPickUp(_reservationId) == false)
        {
            return;
        }

        _carriedResource = resource;

        Transform parent = _holdPoint != null ? _holdPoint : transform;
        _carriedResource.transform.SetParent(parent);

        _carriedResource.PickUp();

        if (TakeObject != null)
        {
            TakeObject.Invoke(_carriedResource);
        }
    }

    public void ClearTarget()
    {
        if (_carriedResource == null && _expectedResource != null)
        {
            _expectedResource.CancelReservation(_reservationId);
        }

        _expectedResource = null;
        _reservationId = default;
    }

    public void Release()
    {
        if (_carriedResource != null)
        {
            _carriedResource.Release();
        }

        _carriedResource = null;
        ClearTarget();
    }
}
