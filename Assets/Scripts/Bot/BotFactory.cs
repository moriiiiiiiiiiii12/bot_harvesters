using System;
using UnityEngine;

public class BotFactory : Factory
{
    [SerializeField] private SpawnerRef _spawnerBotRef;
    private SpawnerBot _spawnerBot;

    public event Action<Bot> BotCreate;

    protected override void OnEnable()
    {
        base.OnEnable();

        _spawnerBot = (SpawnerBot)_spawnerBotRef.Value;
    }

    public override void Produce()
    {
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
