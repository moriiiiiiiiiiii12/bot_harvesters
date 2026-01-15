using System;
using UnityEngine;

public class BaseFactory : Factory<Base>
{
    [SerializeField] private SpawnerBase _spawnerBase;

    public event Action<Base> BaseCreated;

    public void Init(SpawnerBase spawnerBase)
    {
        _spawnerBase = spawnerBase;
    }

    protected override Base CreateInternal(Vector3 position)
    {
        Base createdBase = _spawnerBase.TrySpawnOne(position);

        if (createdBase == null)
        {
            return null;
        }

        if (BaseCreated != null)
        {
            BaseCreated.Invoke(createdBase);
        }

        return createdBase;
    }
}
