using System.Collections.Generic;
using UnityEngine;

public class ScannerResources : MonoBehaviour
{
    [SerializeField] private string _resourceTag = "Resource";

    private readonly List<Resource> _resources = new List<Resource>();

    private void OnEnable()
    {
        Rebuild();
    }

    private void OnDisable()
    {
        _resources.Clear();
    }

    public bool TryGetResource(out Resource resource)
    {
        resource = null;

        if (_resources.Count == 0)
        {
            Rebuild();
        }

        while (_resources.Count > 0)
        {
            Resource resourceCandidate = _resources[0];
            _resources.RemoveAt(0);

            if (resourceCandidate != null && resourceCandidate.Reserved == false)
            {
                resourceCandidate.Reserve();
                resource = resourceCandidate;
                
                return true;
            }
        }

        return false;
    }

    private void Rebuild()
    {
        _resources.Clear();

        GameObject[] foundGameObjects;

        foundGameObjects = GameObject.FindGameObjectsWithTag(_resourceTag);

        for (int i = 0; i < foundGameObjects.Length; i++)
        {
            GameObject gameObjectWithTag = foundGameObjects[i];

            if (gameObjectWithTag.TryGetComponent(out Resource resourceComponent) && gameObjectWithTag.activeSelf)
            {
                _resources.Add(resourceComponent);
            }
        }
    }
}