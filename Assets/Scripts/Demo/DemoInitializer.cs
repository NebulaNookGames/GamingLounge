using UnityEngine;
using System.Collections.Generic;

public class DemoInitializer : MonoBehaviour
{
    public List<GameObject> gameObjectsToDeactivate;
    public List<GameObject> lockedGameObjects; 
    private void Awake()
    {
        foreach (GameObject go in gameObjectsToDeactivate)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in lockedGameObjects)
        {
            go.SetActive(true);
        }
       
    }
}