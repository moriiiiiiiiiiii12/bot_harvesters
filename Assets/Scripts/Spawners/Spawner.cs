using System;
using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Префаб: ")]
    [SerializeField] protected T Prefab;

    public event Action<T> Spawned;

    protected void RaiseSpawned(T instance)
    {
        Spawned?.Invoke(instance);
    }

    public abstract T Spawn(Vector3 position, Quaternion rotation);
}
