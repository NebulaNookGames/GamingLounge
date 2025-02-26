using UnityEngine;
using System.Collections.Generic;

public class DemoInitializer : MonoBehaviour
{
    public List<GameObject> gameObjectsToDeactivate;
    public GameObject lockedGameObject; 
    private void Awake()
    {
        foreach (GameObject go in gameObjectsToDeactivate)
        {
            go.SetActive(false);
        }
        
        lockedGameObject.SetActive(true);
    }
}