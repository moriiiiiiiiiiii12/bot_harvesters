using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 0.1f;
    [SerializeField] private CameraInputReader _cameraInput;

    private void Update()
    {
        if (_cameraInput.IsMiddleButtonHeld == false)
        {
            return;
        }

        Vector2 mouseDelta = _cameraInput.MouseDelta;
        Vector3 moveVector = new Vector3(-mouseDelta.x, 0f, -mouseDelta.y) * _moveSpeed;
        transform.position += moveVector;
    }
}
