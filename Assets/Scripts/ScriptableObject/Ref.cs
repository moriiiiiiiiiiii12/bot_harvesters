using UnityEngine;


public abstract class Ref<T> : ScriptableObject where T : MonoBehaviour
{
    [SerializeField] private T _value;

    public T Value => _value;
    
    public void Set(T value)
    {
        _value = value;        
    }
}