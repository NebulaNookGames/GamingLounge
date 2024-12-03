using System;
using UnityEngine;
using System.Collections.Generic;

public class SitPositionRecognition : MonoBehaviour
{
    [SerializeField] private string[] tags;
    public List<GameObject> collidedObjects = new List<GameObject>();
    public List<GameObject> validObjects = new List<GameObject>();
    private void Awake()
    {
        SurfaceReBake.instance.OnRebake += FillValidObjectList;
    }

    private void OnDestroy()
    {
        SurfaceReBake.instance.OnRebake -= FillValidObjectList;
    }

    public void OnTriggerEnter(Collider other)
    {
        foreach (string tagToCheck in tags)
        {
            if (other.CompareTag(tagToCheck))
            {
                collidedObjects.Add(other.gameObject);
            }
        }
        FillValidObjectList();
    }

    public void OnTriggerExit(Collider other)
    {
        foreach (string tagToCheck in tags)
        {
            if (other.CompareTag(tagToCheck))
                if (collidedObjects.Contains(other.gameObject))
                    collidedObjects.Remove(other.gameObject);
        }
        FillValidObjectList();
    }

    void FillValidObjectList()
    {
        validObjects.Clear();
        
        foreach (GameObject collidedObject in collidedObjects)
        {
            if (collidedObject == null || !collidedObject.GetComponent<SitPositionAvailability>().available) continue; 
            
            // Calculate the vector pointing from the other object to this object
            Vector3 directionToThis = (transform.parent.position - collidedObject.transform.parent.position).normalized;

            // Check the dot product between the other object's forward vector and the direction to this object
            float dotProduct = Vector3.Dot(collidedObject.transform.parent.forward, directionToThis);

            if (dotProduct < -0.7f)
            {
                if(collidedObject.GetComponent<SitPositionAvailability>().available)
                    validObjects.Add(collidedObject);
            }
        }
    }
}
