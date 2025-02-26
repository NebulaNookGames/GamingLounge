using System;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    private void OnEnable()
    {
        WorldInteractables.instance.pointOfInterests.Add(this);
    }

    private void OnDisable()
    {
        WorldInteractables.instance.pointOfInterests.Remove(this);
    }
}