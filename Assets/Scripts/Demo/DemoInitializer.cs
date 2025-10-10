using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DemoInitializer : MonoBehaviour
{
    // List of GameObjects that will be deactivated when the scene starts
    [SerializeField] public List<GameObject> gameObjectsToDeactivate;

    [SerializeField] public Button expandButton; 

    private void Start()
    {
#if !UNITY_SWITCH
        if (SteamIntegration.instance && SteamIntegration.instance.isFullVersion) return; 
        // Deactivate each GameObject in the gameObjectsToDeactivate list
        foreach (GameObject go in gameObjectsToDeactivate)
        {
            go.SetActive(false);
        }

        expandButton.interactable = false; 
#endif
    }
}