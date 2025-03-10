using UnityEngine;
using System.Collections.Generic;

public class DemoInitializer : MonoBehaviour
{
    // ============================
    // Serialized Fields
    // ============================

    // List of GameObjects that will be deactivated when the scene starts
    [SerializeField] public List<GameObject> gameObjectsToDeactivate;

    // List of GameObjects that will be activated when the scene starts
    [SerializeField] public List<GameObject> lockedGameObjects;

    // ============================
    // Unity Methods
    // ============================

    // Called when the script is initialized (before the first frame update)
    private void Awake()
    {
        // Deactivate each GameObject in the gameObjectsToDeactivate list
        foreach (GameObject go in gameObjectsToDeactivate)
        {
            go.SetActive(false); // Disable the GameObject
        }

        // Activate each GameObject in the lockedGameObjects list
        foreach (GameObject go in lockedGameObjects)
        {
            go.SetActive(true); // Enable the GameObject
        }
    }
}