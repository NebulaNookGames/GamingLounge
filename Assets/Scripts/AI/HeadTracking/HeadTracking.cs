using UnityEngine;
using System.Collections.Generic;
using Unity.Behavior;

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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        graph.BlackboardReference.SetVariableValue("HeadTracker", this.gameObject);
    }

    private void Update()
    {
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
            {
                Debug.Log("Reset 01");
                targetPos = originalPos;
            }
        }
        else
        {
            Debug.Log("Reset 02");
            targetPos = originalPos;
        }
     
        target.position = Vector3.Lerp(target.position, targetPos, Time.deltaTime * retargetSpeed);
    }
}