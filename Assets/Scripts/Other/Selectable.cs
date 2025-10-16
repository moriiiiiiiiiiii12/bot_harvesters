using System;
using UnityEngine;

public abstract class Selectable : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    private Color _originalColor;

    public event Action<Selectable> DeselectionRequested;

    public bool IsSelected { get; private set; }

    protected virtual void Awake()
    {
        if (_renderer != null)
            _originalColor = _renderer.material.color;
    }

    public virtual void Select()
    {
        IsSelected = true;
        if (_renderer != null)
            _renderer.material.color = Color.grey;
    }

    public virtual void Deselect()
    {
        IsSelected = false;
        if (_renderer != null)
            _renderer.material.color = _originalColor;
    }

    protected void RequestDeselect()
    {
        DeselectionRequested?.Invoke(this);
    }

    public virtual void OnInteractWith(Selectable target) { }

    public virtual void OnWorldClick(RaycastHit worldPoint) { }
}
