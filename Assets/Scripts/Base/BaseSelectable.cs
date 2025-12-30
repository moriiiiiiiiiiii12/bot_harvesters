using UnityEngine;

class BaseSelectable : Selectable
{
    private Base _base;

    protected override void Awake()
    {
        base.Awake();
        _base = GetComponent<Base>();
    }

    public override void OnWorldClick(RaycastHit hit)
    {
        Map map;

        if (hit.collider.TryGetComponent(out map) == true)
        {
            _base.RequestExpansion(hit.point);

            RequestDeselect();
        }
    }
}
