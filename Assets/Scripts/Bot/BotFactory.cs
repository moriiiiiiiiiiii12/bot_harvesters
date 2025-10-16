using UnityEngine;


class BotFactory : Factory
{
    [SerializeField] private SpawnerBot _spawnerBot;
    [SerializeField] private Base _base;

    public override void Produce()
    {
        if (_counterResource.Count >= _countResourceProduce)
        {
            Bot bot = _spawnerBot.TrySpawnOne();

            if (bot != null)
            {
                _counterResource.Decrease(_countResourceProduce);
                _base.AddBot(bot);   
            }
        }
    }
} 
