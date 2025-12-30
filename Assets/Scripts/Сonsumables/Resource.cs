using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;

    private int _reservedById;
    private bool _isPickedUp;

    public int Id { get; private set; }

    public bool IsReserved
    {
        get { return _reservedById != 0; }
    }

    public event Action<Resource> ReleaseRequested;

    private void OnEnable()
    {
        _reservedById = default;
        _isPickedUp = false;

        if (_collider != null)
        {
            _collider.enabled = true;
        }

        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = false;
        }

        transform.SetParent(null);

        Id = IdGenerator.GenerateId();
    }

    public bool TryReserve(int reservationId)
    {
        if (_reservedById != 0)
        {
            return _reservedById == reservationId;
        }

        _reservedById = reservationId;
        return true;
    }

    public void CancelReservation(int reservationId)
    {
        if (_reservedById != reservationId)
        {
            return;
        }

        if (_isPickedUp == true)
        {
            return;
        }

        _reservedById = default;
    }

    public bool TryPickUp(int reservationId)
    {
        if (_reservedById != reservationId)
        {
            return false;
        }

        if (_isPickedUp == true)
        {
            return false;
        }

        _isPickedUp = true;
        return true;
    }

    public void PickUp()
    {
        if (_collider != null)
        {
            _collider.enabled = false;
        }

        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
        }

        transform.localPosition = Vector3.zero;
    }

    public void Release()
    {
        if (_collider != null)
        {
            _collider.enabled = true;
        }

        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = false;
        }

        transform.SetParent(null);

        _isPickedUp = false;
    }

    public void Reset()
    {
        _reservedById = default;
        _isPickedUp = false;

        if (_collider != null)
        {
            _collider.enabled = true;
        }

        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = false;
        }

        transform.SetParent(null);

        if (ReleaseRequested != null)
        {
            ReleaseRequested.Invoke(this);
        }
    }
}
