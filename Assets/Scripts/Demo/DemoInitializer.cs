using UnityEngine;
using System.Collections.Generic;

public class DemoInitializer : MonoBehaviour
{
    public List<GameObject> gameObjectsToDeactivate;

    private void Awake()
    {
        foreach (GameObject go in gameObjectsToDeactivate)
        {
            go.SetActive(false);
        }
    }
}