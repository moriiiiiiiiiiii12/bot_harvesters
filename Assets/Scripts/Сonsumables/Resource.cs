using System;
using UnityEngine;


public class Resource : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    public int Id { get; private set; }
    public event Action<Resource> ReleaseRequested;

    private void Awake()
    {
        Id = IdGenerator.GenerateId();
    }

    public void Reset()
    {
        if (ReleaseRequested != null)
        {
            ReleaseRequested(this);
        }
    }

    public void PickUp()
    {
        if (_rigidbody != null)
            _rigidbody.isKinematic = true;

        transform.localPosition = Vector3.zero;
    }

    public void Release()
    {
        if (_rigidbody != null)
            _rigidbody.isKinematic = false;

        gameObject.transform.SetParent(null);
    }
}
