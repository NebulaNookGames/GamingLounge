using UnityEngine;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine.Animations.Rigging;

public class HeadTracking : MonoBehaviour
{
    private List<PointOfInterest> POIs;
    public float radius = 4f;
    public Transform target; 
    public BehaviorGraphAgent graph;
    public float retargetSpeed = 4;
    public bool noTracking;
    public Vector3 originalPos;
    public Vector3 beginPosOffset = Vector3.zero;
    public Rig rig; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        graph.BlackboardReference.SetVariableValue("HeadTracker", this.gameObject);
    }

    private void Update()
    {
        if (noTracking)
        {
            rig.weight = 0;
            return;
        } 
        rig.weight = 1;
        
        originalPos = transform.position + beginPosOffset + (transform.forward * 2f);
        
        // Default target position in world space
        Vector3 targetPos = transform.position + beginPosOffset + (transform.forward * 2f);
        Transform closestTracking = null;
        if (!noTracking)
        {
            float closestDistance = float.MaxValue;

            foreach (PointOfInterest poi in WorldInteractables.instance.pointOfInterests)
            {
                float delta = Vector3.Distance(poi.transform.position, target.position);

                if (delta < radius && delta < closestDistance)
                {
                    closestTracking = poi.transform;
                    closestDistance = delta;
                }
            }

            if (closestTracking != null)
            {
                // Update the desired target position to POI position
                targetPos = closestTracking.position;
            }
        }

        if (closestTracking != null)
        {
            if (Vector3.Distance(closestTracking.position, transform.position) > radius)
                targetPos = originalPos;
        }
        else
            targetPos = originalPos;
     
        target.position = Vector3.Lerp(target.position, targetPos, Time.deltaTime * retargetSpeed);
    }
}