using UnityEngine;

public class BaseProduction : Production
{
    [SerializeField] private BaseFactory _baseFactory;

    private bool _hasRequest;
    private Vector3 _requestedPosition;

    public void Request(Vector3 position)
    {
        _requestedPosition = position;
        _hasRequest = true;

        TryProduce();
    }

    protected override bool TryProduceInternal()
    {
        if (_hasRequest == false)
        {
            return false;
        }

        Base createdBase;
        bool isCreated = _baseFactory.TryCreate(_requestedPosition, out createdBase);

        if (isCreated == false)
        {
            return false;
        }

        _hasRequest = false;
        return true;
    }
}
