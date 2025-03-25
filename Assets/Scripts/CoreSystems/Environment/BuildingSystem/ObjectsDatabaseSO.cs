using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum representing different types of objects that can be placed.
/// </summary>
public enum ObjectType
{
    Ground,
    Wall,
    WallDecor,
    Furniture
};

/// <summary>
/// ScriptableObject that stores a list of object data entries.
/// </summary>
[CreateAssetMenu(menuName = "Objects Database")]
public class ObjectsDatabaseSO : ScriptableObject
{
    /// <summary>
    /// List containing data for each object.
    /// </summary>
    public List<ObjectData> objectsData;
}

/// <summary>
/// Represents the data for a placeable object.
/// </summary>
[Serializable]
public class ObjectData
{
    [Header("General Settings")]
    
    [Tooltip("The name of the object")]
    [field: SerializeField] public string Name { get; private set; }
    
    [Tooltip("The unique ID of the object.")]
    [field: SerializeField] public int ID { get; private set; }
    
    [Tooltip("The size of the object in grid units.")]
    [field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;
    
    [Tooltip("The prefab associated with this object.")]
    [field: SerializeField] public GameObject Prefab { get; private set; }
    
    [Tooltip("The type of the object (e.g., ground, wall).")]
    [field: SerializeField] public ObjectType objectType { get; private set; }
    
    [Tooltip("World resource name.")]
    [field: SerializeField] public string worldResourceName { get; private set; }
    
    [Tooltip("Object cost.")]
    [field: SerializeField] public int cost { get; private set; }
    
    [Header("Use if collisions need to occur for placement to be denied")]
    
    [Tooltip("Should collision be checked through overlapping?")]
    [field: SerializeField] public bool shouldCheckForOverlap { get; private set; }
    
    [Tooltip("The Layermask to use while overlap checking.")]
    [field: SerializeField] public LayerMask overlapCheckingLayermask { get; private set; }
    
    [Header("Use if grid overlap needs to occur for placement to be denied")]
    [Tooltip("Should collision be checked by grid database cell occupation?")]
    [field: SerializeField] public bool shouldCheckForDatabase { get; private set; }
    
    [Tooltip("The type of the objects that deny collisions (e.g., ground, wall).")]
    [field: SerializeField] public ObjectType[] collisionObjectTypes { get; private set; }
    
    [Header("Use if grid overlap needs to occur for placement to be possible")]
    [Tooltip("Should collision be checked by grid database cell occupation?")]
    [field: SerializeField] public bool shouldCheckForAllowCollisions { get; private set; }
    
    [Tooltip("The type of the objects that allow collisions (e.g., ground, wall).")]
    [field: SerializeField] public ObjectType[] allowCollisions { get; private set; }
}