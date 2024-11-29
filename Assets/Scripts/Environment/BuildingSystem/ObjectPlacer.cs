using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the placement and removal of objects in the scene.
/// </summary>
public class ObjectPlacer : MonoBehaviour
{

    [SerializeField] private int arcadeMachineIndex = 12; 
    
    /// <summary>
    /// List of GameObjects that have been placed in the scene.
    /// </summary>
    [SerializeField]
    private List<GameObject> placedGameObjects = new();

    [SerializeField] PlacementSystem placementSystem; 
    
    /// <summary>
    /// Instantiates a prefab at the specified position and rotation.
    /// </summary>
    /// <param name="prefab">The prefab to instantiate.</param>
    /// <param name="position">The position to place the object.</param>
    /// <param name="rotation">The rotation to apply to the object.</param>
    /// <returns>The index of the newly placed object in the list.</returns>
    public int PlaceObject(ObjectData objectData, Vector3 position, Quaternion rotation)
    {
        
        GameObject newObject = Instantiate(objectData.Prefab);
        newObject.transform.position = position;
        if(newObject.GetComponent<RotatePlacementObject>()) 
            newObject.GetComponent<RotatePlacementObject>().objectToRotate.transform.rotation = rotation;
        
        if(newObject.GetComponent<ActivatePlacedObject>()) 
            newObject.GetComponent<ActivatePlacedObject>().Enable();

        if (newObject.GetComponent<AddMoneyOnDestroy>())
            newObject.GetComponent<AddMoneyOnDestroy>().amount = objectData.cost; 
        
        if (objectData.ID == arcadeMachineIndex)
        {
            WorldInteractables.instance.ArcadeMachines.Add(newObject);
            Debug.Log(WorldInteractables.instance.ArcadeMachines.Count.ToString());
        }
        
        MoneyManager.instance.ChangeMoney(-objectData.cost);
        placedGameObjects.Add(newObject);
        placementSystem.OnPlaced?.Invoke();
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

        if (placedGameObjects[gameObjectIndex].CompareTag(("Machine")))
        {
            WorldInteractables.instance.ArcadeMachines.Remove(placedGameObjects[gameObjectIndex]);
            Debug.Log(WorldInteractables.instance.ArcadeMachines.Count.ToString());
        }
        
        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;
        Invoke("ActionInvocation", .2f);
    }

    void ActionInvocation()
    {
        placementSystem.OnPlaced?.Invoke();
    }
}