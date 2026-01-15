using UnityEngine;

public sealed class SpawnerBot : InstantiateSpawner<Bot>
{
    public Bot TrySpawnOne(Vector3 position)
    {
        Bot bot = Spawn(position, Quaternion.identity);
        bot.transform.position = position;
        
        return bot;
    }
}
