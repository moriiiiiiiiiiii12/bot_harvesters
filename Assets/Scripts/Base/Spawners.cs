using UnityEngine;

[CreateAssetMenu]
public class BasePrefab : ScriptableObject
{
    [SerializeField] private Base _basePrefab;

    public Base GetBase()
    {
        return _basePrefab;
    }
}