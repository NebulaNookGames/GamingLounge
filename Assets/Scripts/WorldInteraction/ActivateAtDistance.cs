using System;
using UnityEngine;

public class ActivateAtDistance : MonoBehaviour
{
    GameObject target;
    public GameObject objectToActivate;
    public GameObject objectToCheckDistanceFrom;
    public float distanceToActivate = 1;
    
    private void Start()
    {
       ActivationManager.instance.objectsToCheck.Add(this);
    }

    private void OnDisable()
    {
        ActivationManager.instance.objectsToCheck.Remove(this);
    }

    public void CheckDistance(GameObject target)
    {
        float sqrDist = (objectToCheckDistanceFrom.transform.position - target.transform.position).sqrMagnitude;
        objectToActivate.SetActive(sqrDist <= distanceToActivate * distanceToActivate);
    }
}
