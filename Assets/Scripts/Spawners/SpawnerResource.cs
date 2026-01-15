using System;
using System.Collections;
using UnityEngine;

public sealed class SpawnerResource : PooledSpawner<Resource>
{
    [SerializeField] private Renderer _arenaRenderer;
    [SerializeField] private float _spawnIntervalSeconds = 3.0f;
    [SerializeField] private float _spawnOverlapRadius = 0.5f;
    [SerializeField] private LayerMask _spawnOverlapLayerMask;

    public event Action<Resource> SpawnResource;

    private Coroutine _spawnCoroutine;
    private int _countActiveObjects;
    private int _poolSize = 5;

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

        while (enabled)
        {
            yield return waitForSeconds;

            TrySpawnOne();
        }
    }

    private void TrySpawnOne()
    {
        _poolSize = Pool.MaxSize;

        if (_countActiveObjects >= _poolSize)
        {
            return;
        }

        Bounds bounds = _arenaRenderer.bounds;

        float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
        float z = UnityEngine.Random.Range(bounds.min.z, bounds.max.z);

        Vector3 spawnPosition = new Vector3(x, y, z);

        if (IsSpawnPositionFree(spawnPosition) == false)
        {
            return;
        }

        Resource resource = Spawn(spawnPosition, Quaternion.identity);

        resource.ReleaseRequested -= ReturnObject;
        resource.ReleaseRequested += ReturnObject;

        _countActiveObjects++;

        SpawnResource?.Invoke(resource);
    }

    private bool IsSpawnPositionFree(Vector3 position)
    {
        bool hasOverlap = Physics.CheckSphere(position, _spawnOverlapRadius, _spawnOverlapLayerMask, QueryTriggerInteraction.Ignore);
        
        return hasOverlap == false;
    }

    private void ReturnObject(Resource resource)
    {
        if (Pool.Contains(resource) == false)
        {
            return;
        }

        resource.ReleaseRequested -= ReturnObject;

        _countActiveObjects--;

        Release(resource);
    }
}
