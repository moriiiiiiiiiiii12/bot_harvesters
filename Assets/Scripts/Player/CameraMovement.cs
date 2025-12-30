using System;
using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 0.1f; 
    private CameraControls _controls;
    private Vector2 _mouseDelta;
    private bool _isMiddleButtonHeld;

    private void Awake()
    {
        _controls = new CameraControls();

        _controls.Camera.MouseDelta.performed += ctx => _mouseDelta = ctx.ReadValue<Vector2>();
        _controls.Camera.MouseDelta.canceled += ctx => _mouseDelta = Vector2.zero;

        _controls.Camera.MiddleClick.performed += _ => _isMiddleButtonHeld = true;
        _controls.Camera.MiddleClick.canceled += _ => _isMiddleButtonHeld = false;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Update()
    {
        if (_isMiddleButtonHeld == true)
        {
            transform.position += new Vector3(-_mouseDelta.x, 0, -_mouseDelta.y) * _moveSpeed;
        }
    }

    internal Ray ScreenPointToRay(Vector2 vector2)
    {
        throw new NotImplementedException();
    }
}
