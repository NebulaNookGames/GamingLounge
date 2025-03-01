using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Manages interactable objects in the world, specifically arcade machines.
/// Provides a singleton instance for global access.
/// </summary>
public class WorldInteractables : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the WorldInteractables class.
    /// </summary>
    public static WorldInteractables instance;

    public int machineCount = 0; 
    
    
    public List<GameObject> allAracadeMachines; 
    
    /// <summary>
    /// List of arcade machines available in the world. 
    /// This property is read-only outside this class.
    /// </summary>
    public List<GameObject> availableArcadeMachines;
    
    public List<PointOfInterest> pointOfInterests;

    public GameObject winEffect;

    public List<GameObject> openToChatEntities; 
    
    /// <summary>
    /// Initializes the singleton instance on Awake.
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    public void EndArcadeMachineOccupation(GameObject obj)
    {
        obj.GetComponent<MoneyHolder>().beginVideoPlayer.HandlePlay(false);
        availableArcadeMachines.Add(obj);
    }

    public void OccupyAradeMachine(GameObject obj)
    {
        availableArcadeMachines.Remove(obj);
    }

    public void InitializeNewAracadeMachine(GameObject obj)
    {
        availableArcadeMachines.Add(obj);
        allAracadeMachines.Add(obj);
        machineCount++;
    }

    public void DeleteArcadeMachine(GameObject obj)
    {
        if(allAracadeMachines.Contains(obj))
            allAracadeMachines.Remove(obj);
        if(availableArcadeMachines.Contains(obj))
            availableArcadeMachines.Remove(obj);
        
        machineCount--;
    }
}