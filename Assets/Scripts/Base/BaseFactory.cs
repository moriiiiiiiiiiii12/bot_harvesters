using System;
using UnityEngine;

class BaseFactory : Factory
{
    [SerializeField] private SpawnerBase _spawnerBase;
    [SerializeField] private Transform _flag;

    public event Action<Base> BaseCreate;

    public override void Produce()
    {
        if (_counterResource.Count >= _countResourceProduce)
        {
            Base @base = _spawnerBase.TrySpawnOne(_flag.position);

            _counterResource.Decrease(_countResourceProduce);

            BaseCreate?.Invoke(@base);
        }
    }
}
