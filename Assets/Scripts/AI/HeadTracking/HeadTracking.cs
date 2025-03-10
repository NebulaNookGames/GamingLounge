using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;

public class HeadTracking : MonoBehaviour
{
    #region Serialized Fields

    [Header("Tracking Settings")]
    [Tooltip("The radius within which POIs are considered.")]
    public float radius = 6f; // The radius to check for Points of Interest (POIs)
    
    [Tooltip("Target to be tracked by the head.")]
    public Transform target; // The target to track
    
    [Tooltip("VisitorEntity to which this head tracking belongs.")]
    public VisitorEntity entity; // Reference to the entity associated with this head tracking
    
    [Tooltip("Speed at which the head moves towards the target.")]
    public float retargetSpeed = 4f; // Speed at which the head turns towards the target
    
    [Tooltip("If true, disables head tracking.")]
    public bool noTracking; // Disables tracking if set to true
    
    [Tooltip("Initial position of the NPC (used when no tracking).")]
    public Vector3 originalPos; // The original position (used for fallback when no POI is found)
    
    [Tooltip("Offset to the initial position when tracking begins.")]
    public Vector3 beginPosOffset = Vector3.zero; // Offset for the starting position of the NPC
    
    [Tooltip("Rig used for head tracking.")]
    public Rig rig; // The rig used to control the head movement
    
    [Tooltip("Weight of the rig (influences the head's movement).")]
    public float rigWeight = 0f; // Weight of the rig for head movement
    
    [Tooltip("Time interval between updates.")]
    public float updateInterval = 0; // The interval at which to update tracking

    [Tooltip("Maximum angle for the head to rotate towards a POI.")]
    public float maxAngle = 70f; // Max angle to track a POI
    
    [Tooltip("Point of interest to ignore during tracking.")]
    public PointOfInterest ignorePointOfInterest; // Specific POI to be ignored
    
    [Tooltip("A specific target to be tracked.")]
    public PointOfInterest specificTarget; // Specific target to track, overrides POIs

    #endregion Serialized Fields

    #region Private Variables

    private float updateTimer; // Timer to manage update intervals
    private List<PointOfInterest> POIs; // List of Points of Interest (POIs)

    #endregion Private Variables

    #region Unity Methods

    /// <summary>
    /// Initializes the head tracking and sets up the update timer.
    /// </summary>
    void Start()
    {
        if (entity != null)
            entity.headTracking = this; // Assign this head tracking instance to the entity
        
        updateTimer = updateInterval; // Initialize the update timer
    }

    /// <summary>
    /// Updates the head tracking logic each frame.
    /// </summary>
    private void Update()
    {
        updateTimer -= Time.deltaTime; // Decrease the timer by time passed since last frame
        
        if (updateTimer <= 0)
        {
            updateTimer = updateInterval; // Reset the update timer

            if (noTracking)
            {
                rigWeight = 0; // Reset rig weight if tracking is disabled
                target.position = originalPos; // Set target back to the original position
                rig.weight = Mathf.Lerp(rig.weight, rigWeight, Time.deltaTime); // Smooth transition of the rig weight
                return;
            }

            originalPos = transform.position + beginPosOffset + (transform.forward * 2f); // Update the original position offset

            Vector3 targetPos = transform.position + beginPosOffset + (transform.forward * 2f); // Default target position in front of the NPC
            Transform closestTracking = null; // The closest point of interest (POI)

            // If no specific target is assigned, look for the closest POI
            if (specificTarget == null)
            {
                if (!noTracking)
                {
                    float closestDistance = float.MaxValue;

                    // Loop through all POIs to find the closest one within the specified radius
                    foreach (PointOfInterest poi in WorldInteractables.instance.pointOfInterests)
                    {
                        if (poi == ignorePointOfInterest) continue; // Skip the ignored POI

                        float delta = Vector3.Distance(poi.transform.position, target.position); // Calculate the distance to the POI
                        
                        // If the POI is within the radius and closer than the current closest POI
                        if (delta < radius && delta < closestDistance)
                        {
                            float angle = Vector3.Angle(transform.forward, poi.transform.position - transform.position); // Calculate the angle to the POI
                            
                            // If the angle is within the maxAngle, select the POI as the target
                            if (angle < maxAngle)
                            {
                                closestTracking = poi.transform; // Set the closest POI as the target
                                closestDistance = delta; // Update the closest distance
                            }
                        }
                    }

                    // If a POI is found, start tracking it
                    if (closestTracking != null)
                    {
                        rigWeight = 1; // Set rig weight to 1 to track the POI
                        targetPos = closestTracking.position; // Update target position to the POI position
                    }
                    else
                    {
                        rigWeight = 0; // If no POI is found, reset the rig weight
                    }
                }
            }
            else
            {
                // If a specific target is set, use it
                closestTracking = specificTarget.transform;
            }

            // If the closest POI is too far, stop tracking it
            if (closestTracking != null && Vector3.Distance(closestTracking.position, transform.position) > radius)
            {
                rigWeight = 0; // Reset rig weight
                targetPos = originalPos; // Set target back to original position
            }
            else
            {
                targetPos = originalPos; // Fallback to the original position if no valid POI is found
            }

            // Smoothly update the target position and rig weight
            target.position = Vector3.Lerp(target.position, targetPos, Time.deltaTime * retargetSpeed);
            rig.weight = Mathf.Lerp(rig.weight, rigWeight, Time.deltaTime); // Smooth transition of the rig weight
        }
    }

    #endregion Unity Methods
}
