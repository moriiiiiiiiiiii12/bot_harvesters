using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerResources : MonoBehaviour
{
    [SerializeField] private string _resourceTag = "Resource";
    [SerializeField] private float _scanRadius = 10f;
    [SerializeField] private float _scanInterval = 1f;
    [SerializeField] private LayerMask _resourceLayerMask;
    [SerializeField] private ResourceStorageRef _resourceStorageRef;

    private readonly List<Resource> _detectedResources = new List<Resource>();
    private ResourceStorage _resourceStorage;
    private Coroutine _scanCoroutine;

    private void OnEnable()
    {
        _resourceStorage = _resourceStorageRef.Value;

        UpdateResourceLists();

        _scanCoroutine = StartCoroutine(ScanCoroutine());
    }

    private void OnDisable()
    {
        if (_scanCoroutine != null)
        {
            StopCoroutine(_scanCoroutine);
            _scanCoroutine = null;
        }

        _detectedResources.Clear();
    }

    public bool TryGetResource(out Resource resource)
    {
        resource = null;

        Vector3 scannerPosition = transform.position;
        float scanRadiusSqr = _scanRadius * _scanRadius;

        for (int index = 0; index < _detectedResources.Count; index++)
        {
            Resource detectedResource = _detectedResources[index];

            if (detectedResource.gameObject.activeInHierarchy == false)
            {
                continue;
            }

            Vector3 offset = detectedResource.transform.position - scannerPosition;

            if (offset.sqrMagnitude > scanRadiusSqr)
            {
                continue;
            }

            if (_resourceStorage.TryReserve(detectedResource) == false)
            {
                continue;
            }

            resource = detectedResource;
            
            return true;
        }

        return false;
    }

    private IEnumerator ScanCoroutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_scanInterval);

        while (enabled == true)
        {
            yield return waitForSeconds;

            UpdateResourceLists();
        }
    }

    private void UpdateResourceLists()
    {
        _detectedResources.Clear();

        Collider[] detectedColliders = Physics.OverlapSphere(transform.position, _scanRadius, _resourceLayerMask);

        for (int index = 0; index < detectedColliders.Length; index++)
        {
            Collider collider = detectedColliders[index];

            if (collider.CompareTag(_resourceTag) == false)
            {
                continue;
            }

            Resource resourceComponent;

            if (collider.TryGetComponent<Resource>(out resourceComponent) == false)
            {
                continue;
            }

            if (resourceComponent.gameObject.activeInHierarchy == false)
            {
                continue;
            }

            _detectedResources.Add(resourceComponent);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }
#endif
}
