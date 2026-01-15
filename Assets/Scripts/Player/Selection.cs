using UnityEngine;
using UnityEngine.InputSystem;

public class Selection : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    private CameraControls _controls;
    private Selectable _currentSelection;

    private void Awake()
    {
        _mainCamera = Camera.main;

        _controls = new CameraControls();
        _controls.Camera.LeftClick.performed += _ => OnClick();
    }

    private void OnEnable() => _controls.Enable();
    private void OnDisable() => _controls.Disable();

    private void OnClick()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit) == false)
            return;

        if (hit.collider.TryGetComponent(out Selectable target))
        {
            SelectTarget(target);
            
            return;
        }

        _currentSelection?.OnWorldClick(hit);
    }

    private void SelectTarget(Selectable target)
    {
        if (_currentSelection == target)
        {
            _currentSelection.OnInteractWith(target);
            return;
        }

        ClearSelection();

        _currentSelection = target;
        _currentSelection.DeselectionRequested += OnDeselectionRequested;
        _currentSelection.Select();
    }

    private void OnDeselectionRequested(Selectable sender)
    {
        if (_currentSelection == sender)
            ClearSelection();
    }

    private void ClearSelection()
    {
        if (_currentSelection == null)
            return;

        _currentSelection.DeselectionRequested -= OnDeselectionRequested;
        _currentSelection.Deselect();
        _currentSelection = null;
    }
}
