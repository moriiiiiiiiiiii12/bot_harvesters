using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;

    private bool _isPickedUp;

    public event Action<Resource> ReleaseRequested;

    private void OnEnable()
    {
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
    }

    public bool TryPickUp()
    {
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
