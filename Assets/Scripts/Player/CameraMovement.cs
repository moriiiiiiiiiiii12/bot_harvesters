using System;
using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    private CameraControls controls;
    private Vector2 mouseDelta;
    private bool isMiddleButtonHeld;

    public float moveSpeed = 0.1f; 

    private void Awake()
    {
        controls = new CameraControls();

        controls.Camera.MouseDelta.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
        controls.Camera.MouseDelta.canceled += ctx => mouseDelta = Vector2.zero;

        controls.Camera.MiddleClick.performed += _ => isMiddleButtonHeld = true;
        controls.Camera.MiddleClick.canceled += _ => isMiddleButtonHeld = false;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        if (isMiddleButtonHeld)
        {
            transform.position += new Vector3(-mouseDelta.x, 0, -mouseDelta.y) * moveSpeed;
        }
    }

    internal Ray ScreenPointToRay(Vector2 vector2)
    {
        throw new NotImplementedException();
    }
}
