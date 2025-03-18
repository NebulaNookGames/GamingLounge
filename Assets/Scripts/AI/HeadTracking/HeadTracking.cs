using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;

public class HeadTracking : MonoBehaviour
{
    private List<PointOfInterest> POIs;
    public float radius = 6f;
    public Transform target; 
    public VisitorEntity entity;
    public float retargetSpeed = 4;
    public bool noTracking;
    public Vector3 originalPos;
    public Vector3 beginPosOffset = Vector3.zero;
    public Rig rig;
    public float rigWeight = 0f; 
    public float updateInterval = 0;
    private float updateTimer;
    public float maxAngle = 70; 
    public PointOfInterest ignorePointOfInterest;

    public PointOfInterest specificTarget; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(entity != null)
            entity.headTracking = this;
        
        updateTimer = updateInterval;
    }

    private void Update()
    {
        updateTimer -= Time.deltaTime;

        if (updateTimer <= 0)
        {
            updateTimer = updateInterval;

            if (noTracking)
            {
                rigWeight = 0;
                target.position = originalPos;
                rig.weight = Mathf.Lerp(rig.weight, rigWeight, Time.deltaTime);
                return;
            }

            originalPos = transform.position + beginPosOffset + (transform.forward * 2f);
            
            // Default target position in world space
            Vector3 targetPos = transform.position + beginPosOffset + (transform.forward * 2f);
            Transform closestTracking = null;
            if (specificTarget == null)
            {
                if (!noTracking)
                {
                    float closestDistance = float.MaxValue;


                    foreach (PointOfInterest poi in WorldInteractables.instance.pointOfInterests)
                    {
                        if (poi == ignorePointOfInterest) continue;

                        float delta = Vector3.Distance(poi.transform.position, target.position);




                        if (delta < radius && delta < closestDistance)
                        {
                            float angle = Vector3.Angle(transform.forward, poi.transform.position - transform.position);


                            if (angle < maxAngle)
                            {
                                closestTracking = poi.transform;
                                closestDistance = delta;
                            }
                        }
                    }
                    
                    if (closestTracking != null)
                    {
                        rigWeight = 1;
                        // Update the desired target position to POI position
                        targetPos = closestTracking.position;
                    }
                    else
                    {
                        rigWeight = 0;
                    }
                }
            }
            else
            {
                closestTracking = specificTarget.transform;
            }


            if (closestTracking != null)
            {
                if (Vector3.Distance(closestTracking.position, transform.position) > radius)
                {
                    rigWeight = 0;
                    targetPos = originalPos;
                }
            }
            else
            {
                targetPos = originalPos;
            }
            target.position = Vector3.Lerp(target.position, targetPos, Time.deltaTime * retargetSpeed);
            rig.weight = Mathf.Lerp(rig.weight, rigWeight, Time.deltaTime);
        }
    }
}