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
        if (hit.collider.TryGetComponent(out Map map))
        {
            _base.SetFlag(hit.point);

            RequestDeselect();
        }
    }
}
