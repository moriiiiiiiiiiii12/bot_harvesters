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

    private Coroutine _scanCoroutine;
    private List<Resource> _detectedResources;
    private ResourceStorage _resourceStorage;

    private void OnEnable()
    {
        _scanCoroutine = StartCoroutine(ScanCoroutine());

        _resourceStorage = _resourceStorageRef.Value;
    }

    private void OnDisable()
    {
        if (_scanCoroutine != null)
        {
            StopCoroutine(_scanCoroutine);
        }
    }

    public bool TryGetResource(out Resource resource)
    {
        resource = null;

        foreach(Resource resource1 in _detectedResources)
        {
            if(_resourceStorage.TryReserve(resource1) == true)
            {
                resource = resource1;

                return true;
            }
        }

        return false;
    }

    private IEnumerator ScanCoroutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_scanInterval);

        while (enabled == true)
        {
            UpdateResourceLists();
            
            yield return waitForSeconds;
        }
    }

    private void UpdateResourceLists()
    {
        Collider[] detectedColliders = Physics.OverlapSphere(transform.position, _scanRadius, _resourceLayerMask);

        _detectedResources = new List<Resource>();

        for (int i = 0; i < detectedColliders.Length; i++)
        {
            Collider collider = detectedColliders[i];

            if (collider.CompareTag(_resourceTag) == false)
            {
                continue;
            }

            if (collider.TryGetComponent(out Resource resourceComponent) == true)
            {
                if (resourceComponent.gameObject.activeSelf == true)
                {
                    _detectedResources.Add(resourceComponent);
                }
            }
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
