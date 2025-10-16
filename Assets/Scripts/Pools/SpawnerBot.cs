using System;
using System.Collections;
using UnityEngine;


public class SpawnerBot : Spawner<Bot>
{
    [SerializeField] private Transform _spawnPoint;

    public Bot TrySpawnOne()
    {
        if (CountActiveObjects >= PoolSize)
        {
            return null;
        }

        Bot bot = Pool.Get();

        bot.transform.position = _spawnPoint.position;

        return bot;
    }
}
