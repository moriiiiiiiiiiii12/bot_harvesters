using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 0.1f;

    private CameraControls _controls;
    private Vector2 _mouseDelta;
    private bool _isMiddleButtonHeld;

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

    private void Update()
    {
        if (_isMiddleButtonHeld == false)
        {
            return;
        }

        Vector3 moveVector = new Vector3(-_mouseDelta.x, 0f, -_mouseDelta.y) * _moveSpeed;
        transform.position += moveVector;
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
