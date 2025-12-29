using System;
using UnityEngine;

public class BotFactory : Factory
{
    [SerializeField] private SpawnerBot _spawnerBot;

    public event Action<Bot> BotCreate;

    public override void Produce()
    {
        if (_counterResource.Count >= _countResourceProduce)
        {
            Bot createdBot = _spawnerBot.TrySpawnOne();

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
