using UnityEngine;

abstract class Factory : MonoBehaviour
{
    [SerializeField] protected CounterResource _counterResource;
    [SerializeField] protected int _countResourceProduce = 3;

    protected virtual void OnEnable()
    {
        _counterResource.CountChanged += Produce;
    }

    protected virtual void OnDisable()
    {
        _counterResource.CountChanged -= Produce;
    }

    public abstract void Produce();
}
