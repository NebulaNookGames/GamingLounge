using System;
using UnityEngine;

public class RemoveComponentAfterDuration : MonoBehaviour
{
    public Component component;

    private void Awake()
    {
        Invoke(nameof(RemoveComponent), 2f);
    }

    void RemoveComponent()
    {
        Destroy(component);
    }
}