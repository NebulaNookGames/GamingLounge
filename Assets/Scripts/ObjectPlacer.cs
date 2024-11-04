using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the placement and removal of game objects in the scene.
/// </summary>
public class ObjectPlacer : MonoBehaviour
{
    /// <summary>
    /// A list of currently placed game objects in the scene.
    /// </summary>
    [SerializeField] List<GameObject> placedGameObjects = new();

    /// <summary>
    /// Places a new object in the scene at the specified position using the given prefab.
    /// </summary>
    /// <param name="prefab">The prefab of the object to place.</param>
    /// <param name="position">The world position where the object should be instantiated.</param>
    /// <returns>The index of the newly placed object in the <see cref="placedGameObjects"/> list.</returns>
    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab); // Create a new instance of the prefab
        newObject.transform.position = position; // Set the position of the new object
        placedGameObjects.Add(newObject); // Add the new object to the list of placed objects
        return placedGameObjects.Count - 1; // Return the index of the newly placed object
    }

    /// <summary>
    /// Removes the object at the specified index from the list of placed objects.
    /// </summary>
    /// <param name="gameObjectIndex">The index of the object to remove.</param>
    internal void RemoveObjectAt(int gameObjectIndex)
    {
        // Check if the index is valid and the object exists
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
            return;

        Destroy(placedGameObjects[gameObjectIndex]); // Destroy the object in the scene
        placedGameObjects[gameObjectIndex] = null; // Mark the index as null to indicate removal
    }
}