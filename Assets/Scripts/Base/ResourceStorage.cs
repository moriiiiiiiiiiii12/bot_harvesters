using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    [SerializeField] private SpawnerResource _spawnerResource;
    [SerializeField] private ResourceStorageRef _resourceStorageRef;

    private readonly Dictionary<Resource, bool> _isReservedByResource = new Dictionary<Resource, bool>();

    private void Awake()
    {
        _resourceStorageRef.Set(this);
    }

    private void OnEnable()
    {
        _spawnerResource.CreateObject += AddResource;
    }

    private void OnDisable()
    {
        _spawnerResource.CreateObject -= AddResource;
    }

    private void AddResource(Resource resource)
    {
        if (_isReservedByResource.ContainsKey(resource) == true)
        {
            return;
        }

        _isReservedByResource.Add(resource, false);
    }

    public bool TryReserve(Resource resource)
    {
        if (_isReservedByResource.ContainsKey(resource) == false)
        {
            return false;
        }

        if (_isReservedByResource[resource] == true)
        {
            return false;
        }

        _isReservedByResource[resource] = true;
        
        return true;
    }

    public void Unreserve(Resource resource)
    {
        if (_isReservedByResource.ContainsKey(resource) == false)
        {
            return;
        }

        if (_isReservedByResource[resource] == false)
        {
            return;
        }

        _isReservedByResource[resource] = false;
    }
}
