using UnityEngine;

public class BotProductionPermission : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private int _maxCountBot = 3;

    public bool CanProduce()
    {
        return _base.CurrentCountBots < _maxCountBot;
    }
}
