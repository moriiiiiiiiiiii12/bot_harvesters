using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public abstract class Pool : MonoBehaviour { }

public abstract class Pool<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Необходимые компоненты: ")]
    [SerializeField] private T _prefab;

    [Header("Настройки пула: ")]
    [SerializeField] private int _poolSize = 5;

    private readonly List<T> _activeObjects = new List<T>();
    private ObjectPool<T> _pool;

    public event Action<T> Created;

    public int MaxSize => _poolSize;
    public int CountActive { get; private set; }

    protected virtual void Awake()
    {
        CountActive = 0;

        _pool = new ObjectPool<T>(
            createFunc: CreateInstance,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: OnDestroyObject,
            collectionCheck: true,
            defaultCapacity: _poolSize,
            maxSize: _poolSize
        );
    }

    public T Get()
    {
        return _pool.Get();
    }

    public void Release(T instance)
    {
        _pool.Release(instance);
    }

    public bool Contains(T instance)
    {
        return _activeObjects.Contains(instance) == true;
    }

    private T CreateInstance()
    {
        T instance = Instantiate(_prefab);
        instance.gameObject.SetActive(false);

        Created?.Invoke(instance);

        return instance;
    }

    private void OnGet(T instance)
    {
        CountActive++;
        OnTaken(instance);
        _activeObjects.Add(instance);
    }

    private void OnRelease(T instance)
    {
        CountActive--;
        OnReturned(instance);
        _activeObjects.Remove(instance);
    }

    private void OnDestroyObject(T instance)
    {
        Destroy(instance.gameObject);
    }

    protected virtual void OnTaken(T instance)
    {
        instance.gameObject.SetActive(true);
    }

    protected virtual void OnReturned(T instance)
    {
        instance.gameObject.SetActive(false);
    }
}
