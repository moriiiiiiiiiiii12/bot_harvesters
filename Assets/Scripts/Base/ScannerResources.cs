using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScannerResources : MonoBehaviour
{
    [SerializeField] private string _resourceTag = "Resource";
    [SerializeField] private float _scanRadius = 10f;
    [SerializeField] private float _scanInterval = 1f;
    [SerializeField] private LayerMask _resourceLayerMask;

    private ResourceStorage _storage = new ResourceStorage();
    private Coroutine _scanCoroutine;

    private void OnEnable()
    {
        _scanCoroutine = StartCoroutine(ScanCoroutine());
    }

    private void OnDisable()
    {
        if (_scanCoroutine != null)
        {
            StopCoroutine(_scanCoroutine);
        }

        _storage.Clear();
    }

    public bool TryGetResource(out Resource resource)
    {
        return _storage.TryDequeue(out resource);
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

        List<Resource> detectedResources = new List<Resource>();

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
                    detectedResources.Add(resourceComponent);
                }
            }
        }

        _storage.RefreshAvailable(detectedResources);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }
#endif
}
