using System;
using UnityEngine;


public class DropPoint : MonoBehaviour
{
    public event Action ResourceReceived;

    public void Receive(Resource resource)
    {
        resource.Reset();

        ResourceReceived?.Invoke();
    }
}