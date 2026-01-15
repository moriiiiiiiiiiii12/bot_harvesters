using UnityEngine;

public abstract class Production : MonoBehaviour
{
    [SerializeField] private CounterResource _counterResource;
    [SerializeField] private int _countResourceProduce = 3;

    protected virtual void OnEnable()
    {
        _counterResource.CountIncreased += TryProduce;
    }

    protected virtual void OnDisable()
    {
        _counterResource.CountIncreased -= TryProduce;
    }

    public void TryProduce()
    {
        if (_counterResource.Count < _countResourceProduce)
        {
            return;
        }

        if (TryProduceInternal() == false)
        {
            return;
        }

        _counterResource.Decrease(_countResourceProduce);
    }

    protected abstract bool TryProduceInternal();
}
