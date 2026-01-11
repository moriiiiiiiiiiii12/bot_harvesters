using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private Transform _flag;

    public Transform FlagTransform => _flag;

    public void Set(Vector3 position)
    {
        _flag.position = position;
        _flag.gameObject.SetActive(true);
    }

    public void Unset()
    {
        _flag.gameObject.SetActive(false);
    }
}
