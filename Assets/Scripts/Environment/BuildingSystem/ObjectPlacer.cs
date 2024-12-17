using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the placement and removal of objects in the scene.
/// </summary>
public class ObjectPlacer : MonoBehaviour
{

    [SerializeField] private int[] arcadeMachineIndexes; 
    
    /// <summary>
    /// List of GameObjects that have been placed in the scene.
    /// </summary>
    [SerializeField]
    public List<KeyValuePair<GameObject, int>> placedGameObjects = new();

    [SerializeField] PlacementSystem placementSystem;
    
    /// <summary>
    /// Instantiates a prefab at the specified position and rotation.
    /// </summary>
    /// <param name="objectData">The data of a placed object type.</param>
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

        foreach (var index in arcadeMachineIndexes)
        {
            if (objectData.ID == index)
            {
                WorldInteractables.instance.AddArcadeMachine(newObject);
                break;
            }
        }

        MoneyManager.instance.ChangeMoney(-objectData.cost);
        placedGameObjects.Add(new KeyValuePair<GameObject, int>(newObject, objectData.ID));
        placementSystem.OnPlaced?.Invoke();
        return placedGameObjects.Count - 1;
    }

    /// <summary>
    /// Removes the object at the specified index from the scene.
    /// </summary>
    /// <param name="gameObjectIndex">The index of the object to remove.</param>
    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex].Key == null)
            return;
        
        if (placedGameObjects[gameObjectIndex].Key.CompareTag(("Machine")))
            WorldInteractables.instance.RemoveArcadeMachine(placedGameObjects[gameObjectIndex].Key);
        
        PlacementSystemDataHandler.Instance.RemoveGameObjectData(gameObjectIndex);
        PlacementSystemDataHandler.Instance.UpdateData();
        
        Destroy(placedGameObjects[gameObjectIndex].Key);
        placedGameObjects.RemoveAt(gameObjectIndex);
        Invoke("ActionInvocation", .2f);
    }

    void ActionInvocation()
    {
        placementSystem.OnPlaced?.Invoke();
    }
}