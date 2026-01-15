using System;
using UnityEngine;

public abstract class Factory<T> : MonoBehaviour where T : MonoBehaviour
{
    public event Action<T> Created;

    public bool TryCreate(Vector3 position, out T created)
    {
        created = CreateInternal(position);

        if (created == null)
        {
            return false;
        }

        if (Created != null)
        {
            Created.Invoke(created);
        }

        return true;
    }

    protected abstract T CreateInternal(Vector3 position);
}
