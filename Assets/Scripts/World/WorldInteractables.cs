using System;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// List of arcade machines available in the world. 
    /// This property is read-only outside this class.
    /// </summary>
    public List<GameObject> ArcadeMachines { get; private set; } = new List<GameObject>();

    /// <summary>
    /// Initializes the singleton instance on Awake.
    /// </summary>
    private void Awake()
    {
        instance = this;
    }
}