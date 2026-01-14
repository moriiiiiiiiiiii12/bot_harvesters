using UnityEngine;


public class SpawnerBase : Spawner<BaseInstaller>
{
    [SerializeField] private ResourceStorage _resourceStorage;
    [SerializeField] private SpawnerBot _spawnerBot;

    public Base TrySpawnOne(Vector3 spawnPoint)
    {
        if (CountActiveObjects >= PoolSize)
        {
            return null;
        }

        BaseInstaller @base = Pool.Get();

        @base.Init(_resourceStorage, this, _spawnerBot);
        @base.transform.position = spawnPoint;

        return @base.gameObject.GetComponent<Base>();
    }
}
