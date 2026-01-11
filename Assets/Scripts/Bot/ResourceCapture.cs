using System;
using UnityEngine;

public class ResourceCapture : MonoBehaviour
{
    [SerializeField] private Transform _holdPoint;

    private Resource _carriedResource;

    public event Action<Resource> TakeObject;

    public bool TryPickUp(Resource resource)
    {
        if (resource.TryPickUp() == false)
        {
            return false;
        }

        _carriedResource = resource;

        Transform parent = _holdPoint;
        _carriedResource.transform.SetParent(parent);

        _carriedResource.PickUp();

        if (TakeObject != null)
        {
            TakeObject.Invoke(_carriedResource);
        }

        return true;
    }

    public void Release()
    {
        if (_carriedResource != null)
        {
            _carriedResource.Release();
        }

        _carriedResource = null;
    }
}
