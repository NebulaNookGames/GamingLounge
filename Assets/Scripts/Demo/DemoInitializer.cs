using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DemoInitializer : MonoBehaviour
{
    // List of GameObjects that will be deactivated when the scene starts
    [SerializeField] public List<GameObject> gameObjectsToDeactivate;

    [SerializeField] public Button expandButton; 
    
    // Called when the script is initialized (before the first frame update)
    private void Awake()
    {
        // Deactivate each GameObject in the gameObjectsToDeactivate list
        foreach (GameObject go in gameObjectsToDeactivate)
        {
            go.SetActive(false);
        }

        expandButton.interactable = false; 
    }
}