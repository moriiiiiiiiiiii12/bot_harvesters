using System.Collections.Generic;

public class ResourceStorage
{
    private readonly List<Resource> _availableResources = new List<Resource>();

    public void Clear()
    {
        _availableResources.Clear();
    }

    public void RefreshAvailable(List<Resource> detectedResources)
    {
        _availableResources.Clear();

        for (int index = 0; index < detectedResources.Count; index++)
        {
            Resource resource = detectedResources[index];

            if (resource == null)
            {
                continue;
            }

            if (resource.IsReserved == true)
            {
                continue;
            }

            _availableResources.Add(resource);
        }
    }

    public bool TryDequeue(int reservationId, out Resource resource)
    {
        for (int index = 0; index < _availableResources.Count; index++)
        {
            Resource candidateResource = _availableResources[index];

            if (candidateResource == null)
            {
                continue;
            }

            if (candidateResource.TryReserve(reservationId) == false)
            {
                continue;
            }

            _availableResources.RemoveAt(index);
            resource = candidateResource;
            return true;
        }

        resource = null;
        return false;
    }
}
