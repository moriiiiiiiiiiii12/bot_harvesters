using System;
using UnityEngine;

public class BotFactory : Factory
{
    [SerializeField] private SpawnerBot _spawnerBot;
    [SerializeField] private BotProductionPermission _productionPermission;

    public event Action<Bot> BotCreate;

    public void Init(SpawnerBot spawnerBot)
    {
        _spawnerBot = spawnerBot;
    }

    public override void Produce()
    {
        if (_productionPermission.CanProduce() == false)
        {
            return;
        }

        if (_counterResource.Count >= _countResourceProduce)
        {
            Bot createdBot = _spawnerBot.TrySpawnOne(transform.position);

            if (createdBot != null)
            {
                _counterResource.Decrease(_countResourceProduce);

                if (BotCreate != null)
                {
                    BotCreate.Invoke(createdBot);
                }
            }
        }
    }
}
