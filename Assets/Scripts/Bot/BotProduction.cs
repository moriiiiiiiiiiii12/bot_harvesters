using UnityEngine;

public class BotProduction : Production
{
    [SerializeField] private BotFactory _botFactory;
    [SerializeField] private BotProductionPermission _productionPermission;

    protected override bool TryProduceInternal()
    {
        if (_productionPermission.CanProduce() == false)
        {
            return false;
        }

        Bot createdBot;
        return _botFactory.TryCreate(transform.position, out createdBot);
    }
}
