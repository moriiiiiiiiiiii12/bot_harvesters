using UnityEngine;

public sealed class SpawnerBase : InstantiateSpawner<BaseInstaller>
{
    [Header("Зависимости для префаба")]
    [SerializeField] private ResourceStorage _resourceStorage;
    [SerializeField] private SpawnerBot _spawnerBot;

    public Base TrySpawnOne(Vector3 spawnPoint)
    {
        BaseInstaller baseInstaller = Spawn(spawnPoint, Quaternion.identity);
        baseInstaller.Init(_resourceStorage, this, _spawnerBot);
        baseInstaller.transform.position = spawnPoint;

        return baseInstaller.Base;
    }
}
