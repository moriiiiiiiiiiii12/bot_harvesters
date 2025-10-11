using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public string UUID { get; private set; }
    public event Action<Resource> ReleaseRequested;

    private void Awake()
    {
        UUID = UUIDGenerator.GenerateUUID();
    }

    public void Reset()
    {
        if (ReleaseRequested != null)
        {
            ReleaseRequested(this);
        }
    }
}
