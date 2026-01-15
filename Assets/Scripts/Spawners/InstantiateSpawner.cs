using UnityEngine;

public abstract class InstantiateSpawner<T> : Spawner<T> where T : MonoBehaviour
{
    public override T Spawn(Vector3 position, Quaternion rotation)
    {
        T instance = Instantiate(Prefab, position, rotation);
        RaiseSpawned(instance);
        return instance;
    }
}
