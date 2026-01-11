using UnityEngine;


public class SpawnerBot : Spawner<Bot>
{
    public Bot TrySpawnOne(Vector3 position)
    {
        if (CountActiveObjects >= PoolSize)
        {
            return null;
        }

        Bot bot = Pool.Get();

        bot.transform.position = position;

        return bot;
    }
}
