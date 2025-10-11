using System;
using UnityEngine;

public class ResourceCapture : MonoBehaviour
{
    [SerializeField] private Transform _holdPoint; 

    private Resource _resource;
    private Rigidbody _resourceRigidbody;

    private string _resourceUUID;

    public event Action<Resource> TakeObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
            Take(resource);
    }

    public void SetResourceUUID(string UUID)
    {
        _resourceUUID = UUID;
    }

    private void Take(Resource resource)
    {
        if (_resource != null) 
            return;

        if (_resourceUUID != resource.UUID)
            return;

        _resource = resource;

        Transform parent = _holdPoint != null ? _holdPoint : transform;

        _resource.transform.SetParent(parent);
        _resource.transform.localPosition = Vector3.zero;

        if (_resource.TryGetComponent(out Rigidbody rb))
        {
            _resourceRigidbody = rb;
            _resourceRigidbody.isKinematic = true;
        }

        TakeObject?.Invoke(_resource);
    }

    public void Release()
    {
        if (_resource == null)
            return;

        if (_resourceRigidbody != null)
            _resourceRigidbody.isKinematic = false;

        _resource.transform.SetParent(null);
        _resourceRigidbody = null;
        _resource = null;
        _resourceUUID = null;
    }
}
