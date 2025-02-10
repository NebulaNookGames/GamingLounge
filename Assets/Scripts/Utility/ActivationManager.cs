using System;
using UnityEngine;
using System.Collections.Generic;

public class ActivationManager : MonoBehaviour
{
    public static ActivationManager instance;

    public List<ActivateAtDistance> objectsToCheck;
    public GameObject target;

    private int batchSize = 20;    // Number of objects to process per frame
    private int currentIndex = 0;  // Tracks where we left off

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void Update()
    {
        if (objectsToCheck.Count == 0 || target == null)
            return;

        // Process a batch of objects each frame
        int objectsProcessed = 0;
        while (objectsProcessed < batchSize && currentIndex < objectsToCheck.Count)
        {
            objectsToCheck[currentIndex].CheckDistance(target);
            currentIndex++;
            objectsProcessed++;
        }

        // Loop back to the start after processing all objects
        if (currentIndex >= objectsToCheck.Count)
            currentIndex = 0;
    }
}
