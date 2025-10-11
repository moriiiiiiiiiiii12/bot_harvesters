using System;
using UnityEngine;


public class ResourceCapture : MonoBehaviour
{
    [SerializeField] private Transform _holdPoint;

    private Resource _resource;

    private int _resourceId;

    public event Action<Resource> TakeObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
            Take(resource);
    }

    public void SetResourceId(int Id)
    {
        _resourceId = Id;
    }

    private void Take(Resource resource)
    {
        if (_resource != null)
            return;

        if (_resourceId != resource.Id)
            return;

        _resource = resource;

        Transform parent = _holdPoint != null ? _holdPoint : transform;

        _resource.transform.SetParent(parent);

        _resource.Take();

        TakeObject?.Invoke(_resource);
    }

    public void Release()
    {
        if (_resource == null)
            return;

        _resource.Release();

        _resource = null;
        _resourceId = default;
    }
}
