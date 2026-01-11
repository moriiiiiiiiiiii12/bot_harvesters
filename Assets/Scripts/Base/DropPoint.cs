using System;
using UnityEngine;


public class DropPoint : MonoBehaviour
{
    public event Action ResourceReceived;
    public event Action<Resource> ResourceDelivered;

    public void Receive(Resource resource)
    {
        resource.Reset();

        ResourceReceived?.Invoke();
        ResourceDelivered?.Invoke(resource);
    }
}