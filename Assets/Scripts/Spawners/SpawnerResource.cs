using System;
using System.Collections;
using UnityEngine;


public class SpawnerResource : Spawner<Resource>
{
    [SerializeField] private Renderer _arenaRenderer;
    [SerializeField] private float _spawnIntervalSeconds = 3.0f;

    public event Action<Resource> SpawnResource;

    private Coroutine _spawnCoroutine;

    private void OnEnable()
    {
        _spawnCoroutine = StartCoroutine(SpawnLoopCoroutine());
    }

    private void OnDisable()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnLoopCoroutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_spawnIntervalSeconds);

        while (enabled == true)
        {
            TrySpawnOne();
            
            yield return waitForSeconds;
        }
    }

    private void TrySpawnOne()
    {
        if (CountActiveObjects >= PoolSize)
        {
            return;
        }

        Resource resource = Pool.Get();

        Bounds bounds = _arenaRenderer.bounds;

        float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
        float z = UnityEngine.Random.Range(bounds.min.z, bounds.max.z);

        resource.transform.position = new Vector3(x, y, z);

        resource.ReleaseRequested += ReturnObject;
        SpawnResource?.Invoke(resource);
    }

    protected void ReturnObject(Resource resource)
    {
        if (resource == null)
            return;

        if (ActiveObjects.Contains(resource) == false)
            return;

        resource.ReleaseRequested -= ReturnObject;
        Pool.Release(resource);
    }
}
