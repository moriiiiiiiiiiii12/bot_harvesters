using System.Collections.Generic;
using UnityEngine;

public class ListResources : MonoBehaviour
{
    [SerializeField] private List<ResourceSpawner> _resourceSpawners;

    private List<Resource> _resources = new List<Resource> {};
    
    private void OnEnable()
    {
        foreach (ResourceSpawner resourceSpawner in _resourceSpawners)
        {
            resourceSpawner.SpawnResource += AddResource;
        }
    }

    private void OnDisable()
    {
        foreach (ResourceSpawner resourceSpawner in _resourceSpawners)
        {
            resourceSpawner.SpawnResource -= AddResource;
        }
    }

    public bool TryGetResource(out Resource resource)
    {
        resource = null;

        if (_resources.Count == 0)
            return false;

        resource = _resources[0];
        _resources.RemoveAt(0);

        return true;
    }

    private void AddResource(Resource resource)
    {
        _resources.Add(resource);
    }
}