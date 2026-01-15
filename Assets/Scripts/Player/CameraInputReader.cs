using UnityEngine;

public abstract class CameraInputReader : MonoBehaviour
{
    public abstract Vector2 MouseDelta { get; }
    public abstract bool IsMiddleButtonHeld { get; }
}
