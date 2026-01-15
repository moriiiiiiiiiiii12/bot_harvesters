using UnityEngine;

public class BaseInstaller : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private ScannerResources _scannerResources;
    [SerializeField] private BaseFactory _baseFactory;
    [SerializeField] private BotFactory _botfactory;

    public Base Base => _base;

    public void Init(ResourceStorage resourceStorage, SpawnerBase spawnerBase, SpawnerBot spawnerBot)
    {
        _base.Init(resourceStorage);
        _scannerResources.Init(resourceStorage);
    
        _baseFactory.Init(spawnerBase);
        _botfactory.Init(spawnerBot);
    } 
}
