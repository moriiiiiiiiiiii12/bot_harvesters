using System;
using System.Collections;
using UnityEngine;


public class SpawnerBase : Spawner<Base>
{
    [SerializeField] private BaseRef _basePrefab;

    protected override void Awake()
    {
        Prefab = _basePrefab.Value;

        base.Awake();
    }

    public Base TrySpawnOne(Vector3 spawnPoint)
    {
        if (CountActiveObjects >= PoolSize)
        {
            return null;
        }

        Base @base = Pool.Get();

        @base.Init();
        @base.transform.position = spawnPoint;

        return @base;
    }
}
