using System;
using UnityEngine;

public class BaseFactory : Factory
{
    [SerializeField] private SpawnerRef _spawnerBaseRef;
    private SpawnerBase _spawnerBase;

    public event Action<Base> BaseCreate;

    private Vector3 _spawnPosition;
    private bool _hasSpawnPosition;

    protected override void OnEnable()
    {
        base.OnEnable(); 

        _spawnerBase = (SpawnerBase)_spawnerBaseRef.Value;
    }

    public void SetSpawnPosition(Vector3 position)
    {
        _spawnPosition = position;
        _hasSpawnPosition = true;
    }

    public void ClearSpawnPosition()
    {
        _hasSpawnPosition = false;
    }

    public override void Produce()
    {
        if (_hasSpawnPosition == false)
        {
            return;
        }

        if (_counterResource.Count >= _countResourceProduce)
        {
            Base createdBase = _spawnerBase.TrySpawnOne(_spawnPosition);

            if (createdBase != null)
            {
                _hasSpawnPosition = false;
                _counterResource.Decrease(_countResourceProduce);

                if (BaseCreate != null)
                {
                    BaseCreate.Invoke(createdBase);
                }
            }
        }
    }
}
