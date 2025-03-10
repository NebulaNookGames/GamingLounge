using System;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    // Whether this point of interest is a character
    public bool isCharacter;

    /// <summary>
    /// Adds this PointOfInterest to the global list of points of interest when it's enabled.
    /// </summary>
    private void OnEnable()
    {
        // Add this PointOfInterest to the WorldInteractables' list of POIs
        WorldInteractables.instance.pointOfInterests.Add(this);
    }

    /// <summary>
    /// Removes this PointOfInterest from the global list of points of interest when it's disabled.
    /// </summary>
    private void OnDisable()
    {
        // Remove this PointOfInterest from the WorldInteractables' list of POIs
        WorldInteractables.instance.pointOfInterests.Remove(this);
    }
}