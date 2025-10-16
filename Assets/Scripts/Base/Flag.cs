using UnityEngine;


class Flag : MonoBehaviour
{
    public void Set(Vector3 position)
    {
        transform.position = position;

        gameObject.SetActive(true);
    }
    
    public void Unset()
    {
        gameObject.SetActive(false);
    }
}