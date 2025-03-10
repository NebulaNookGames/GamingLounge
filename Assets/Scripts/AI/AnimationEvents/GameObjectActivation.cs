using UnityEngine;

/// <summary>
/// This class manages the activation and deactivation of multiple GameObjects.
/// It allows for easily toggling the active state of a list of GameObjects.
/// </summary>
public class GameObjectActivation : MonoBehaviour
{
    [Header("GameObjects Settings")]
    [Tooltip("Array of GameObjects to activate or deactivate.")]
    [SerializeField] private GameObject[] gameObjectsToActivate;

    /// <summary>
    /// Activates all the GameObjects in the gameObjectsToActivate array.
    /// </summary>
    public void Activate()
    {
        foreach (GameObject go in gameObjectsToActivate)
            go.SetActive(true);
    }

    /// <summary>
    /// Deactivates all the GameObjects in the gameObjectsToActivate array.
    /// </summary>
    public void Deactivate()
    {
        foreach (GameObject go in gameObjectsToActivate)
            go.SetActive(false);
    }

    /// <summary>
    /// Toggles the active state of all GameObjects in the gameObjectsToActivate array.
    /// </summary>
    public void Invert()
    {
        foreach (GameObject go in gameObjectsToActivate)
            go.SetActive(!go.activeSelf);
    }
}