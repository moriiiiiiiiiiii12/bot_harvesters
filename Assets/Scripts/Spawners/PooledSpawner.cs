using UnityEngine;

public abstract class PooledSpawner<T> : Spawner<T> where T : MonoBehaviour
{
    [Header("Пул: ")]
    [SerializeField] private Pool<T> _pool;

    protected Pool<T> Pool => _pool;

    public override T Spawn(Vector3 position, Quaternion rotation)
    {
        T instance = _pool.Get();
        instance.transform.SetPositionAndRotation(position, rotation);
        RaiseSpawned(instance);

        return instance;
    }

    protected void Release(T instance)
    {
        _pool.Release(instance);
    }
}
