using System;
using UnityEngine;

/// <summary>
/// Manages the activation and deactivation of objects in the scene.
/// Disables all objects and re-enables an overview panel when deactivated.
/// </summary>
public class ObjectActivator : MonoBehaviour
{
    /// <summary>
    /// The overview panel to be hidden when an object is activated.
    /// </summary>
    [SerializeField] private GameObject overviewPanel;

    /// <summary>
    /// Array of objects that can be activated or deactivated.
    /// </summary>
    [SerializeField] private GameObject[] objects;

    /// <summary>
    /// Activates a specific object by index and hides the overview panel.
    /// </summary>
    /// <param name="objectIndex">Index of the object to activate.</param>
    public void Activate(int objectIndex)
    {
        // Hide the overview panel.
        overviewPanel.SetActive(false);

        // Ensure the index is within bounds.
        if (objectIndex >= objects.Length) return;

        // Activate the selected object.
        objects[objectIndex].SetActive(true);
    }

    /// <summary>
    /// Disables all objects and re-enables the overview panel when this component is disabled.
    /// </summary>
    public void OnDisable()
    {
        // Deactivate all objects in the array.
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }

        // Show the overview panel.
        overviewPanel.SetActive(true);
        
        PlacementSystem.Instance.StopPlacement();
    }
}