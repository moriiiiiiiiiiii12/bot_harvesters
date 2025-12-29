using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private Flag _flag;

    public void Set(Vector3 position)
    {
        _flag.transform.position = position;
        _flag.gameObject.SetActive(true);
    }

    public void Unset()
    {
        _flag.gameObject.SetActive(false);
    }
}
