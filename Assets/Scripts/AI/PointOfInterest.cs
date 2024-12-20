using System;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    private void Awake()
    {
        WorldInteractables.instance.pointOfInterests.Add(this);
    }

    private void OnDestroy()
    {
        WorldInteractables.instance.pointOfInterests.Remove(this);
    }
}
