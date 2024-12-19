using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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

    public List<GameObject> allAracadeMachines; 
    
    /// <summary>
    /// List of arcade machines available in the world. 
    /// This property is read-only outside this class.
    /// </summary>
    public List<GameObject> availableArcadeMachines;
    
    /// <summary>
    /// Initializes the singleton instance on Awake.
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    public void EndArcadeMachineOccupation(GameObject obj)
    {
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
    }

    public void DeleteArcadeMachine(GameObject obj)
    {
        allAracadeMachines.Remove(obj);
        availableArcadeMachines.Remove(obj);
    }
}