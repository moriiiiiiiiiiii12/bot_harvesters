using UnityEngine;

public abstract class Factory : MonoBehaviour
{
    [SerializeField] protected CounterResource _counterResource;
    [SerializeField] protected int _countResourceProduce = 3;

    protected virtual void OnEnable()
    {
        _counterResource.CountIncreased += Produce;
    }

    protected virtual void OnDisable()
    {
        _counterResource.CountIncreased -= Produce;
    }

    public abstract void Produce();
}
