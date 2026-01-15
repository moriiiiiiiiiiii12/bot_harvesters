using System;
using UnityEngine;

public class BotFactory : Factory<Bot>
{
    [SerializeField] private SpawnerBot _spawnerBot;

    public event Action<Bot> BotCreate;

    public void Init(SpawnerBot spawnerBot)
    {
        _spawnerBot = spawnerBot;
    }


    protected override Bot CreateInternal(Vector3 position)
    {
        Bot createdBot = _spawnerBot.TrySpawnOne(position);

        if (createdBot == null)
        {
            return null;
        }

        if (BotCreate != null)
        {
            BotCreate.Invoke(createdBot);
        }

        return createdBot;
    }
}
