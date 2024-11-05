using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the placement and removal of objects in the scene.
/// </summary>
public class ObjectPlacer : MonoBehaviour
{
    /// <summary>
    /// List of GameObjects that have been placed in the scene.
    /// </summary>
    [SerializeField]
    private List<GameObject> placedGameObjects = new();

    /// <summary>
    /// Instantiates a prefab at the specified position and rotation.
    /// </summary>
    /// <param name="prefab">The prefab to instantiate.</param>
    /// <param name="position">The position to place the object.</param>
    /// <param name="rotation">The rotation to apply to the object.</param>
    /// <returns>The index of the newly placed object in the list.</returns>
    public int PlaceObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        newObject.GetComponent<RotatePlacementObject>().objectToRotate.transform.rotation = rotation;
        
        placedGameObjects.Add(newObject);
        return placedGameObjects.Count - 1;
    }

    /// <summary>
    /// Removes the object at the specified index from the scene.
    /// </summary>
    /// <param name="gameObjectIndex">The index of the object to remove.</param>
    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
            return;

        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;
    }
}