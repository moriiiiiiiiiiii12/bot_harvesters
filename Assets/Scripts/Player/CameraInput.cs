using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInput : CameraInputReader
{
    private CameraControls _controls;
    private Vector2 _mouseDelta;
    private bool _isMiddleButtonHeld;

    public override Vector2 MouseDelta
    {
        get { return _mouseDelta; }
    }

    public override bool IsMiddleButtonHeld
    {
        get { return _isMiddleButtonHeld; }
    }

    private void Awake()
    {
        _controls = new CameraControls();
    }

    private void OnEnable()
    {
        _controls.Enable();

        _controls.Camera.MouseDelta.performed += OnMouseDeltaPerformed;
        _controls.Camera.MouseDelta.canceled += OnMouseDeltaCanceled;

        _controls.Camera.MiddleClick.performed += OnMiddleClickPerformed;
        _controls.Camera.MiddleClick.canceled += OnMiddleClickCanceled;
    }

    private void OnDisable()
    {
        _controls.Camera.MouseDelta.performed -= OnMouseDeltaPerformed;
        _controls.Camera.MouseDelta.canceled -= OnMouseDeltaCanceled;

        _controls.Camera.MiddleClick.performed -= OnMiddleClickPerformed;
        _controls.Camera.MiddleClick.canceled -= OnMiddleClickCanceled;

        _controls.Disable();
    }

    private void OnMouseDeltaPerformed(InputAction.CallbackContext callbackContext)
    {
        _mouseDelta = callbackContext.ReadValue<Vector2>();
    }

    private void OnMouseDeltaCanceled(InputAction.CallbackContext callbackContext)
    {
        _mouseDelta = Vector2.zero;
    }

    private void OnMiddleClickPerformed(InputAction.CallbackContext callbackContext)
    {
        _isMiddleButtonHeld = true;
    }

    private void OnMiddleClickCanceled(InputAction.CallbackContext callbackContext)
    {
        _isMiddleButtonHeld = false;
    }
}
