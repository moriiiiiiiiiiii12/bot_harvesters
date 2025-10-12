using System.Collections.Generic;


public class ResourceStorage
{
    private readonly List<Resource> _availableResources = new List<Resource>();
    private readonly List<Resource> _reservedResources = new List<Resource>();

    public void Clear()
    {
        foreach (Resource resource in _reservedResources)
            resource.ReleaseRequested -= OnResourceReleased;

        _availableResources.Clear();
        _reservedResources.Clear();
    }

    public bool TryDequeue(out Resource resource)
    {
        resource = null;

        if (_availableResources.Count == 0)
            return false;

        int indexSelectResource = 0;

        resource = _availableResources[indexSelectResource];
        _availableResources.RemoveAt(indexSelectResource);

        _reservedResources.Add(resource);
        resource.ReleaseRequested += OnResourceReleased;

        return true;
    }

    private void OnResourceReleased(Resource resource)
    {
        resource.ReleaseRequested -= OnResourceReleased;
        _reservedResources.Remove(resource);
    }

    public void RefreshAvailable(List<Resource> detectedResources)
    {
        _availableResources.Clear();

        foreach (Resource resource in detectedResources)
        {
            if (_reservedResources.Contains(resource) == false)
                _availableResources.Add(resource);
        }
    }
}
