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
    /// <summary>
    /// The name of the object.
    /// </summary>
    [field: SerializeField]
    public string Name { get; private set; }

    /// <summary>
    /// The unique ID of the object.
    /// </summary>
    [field: SerializeField]
    public int ID { get; private set; }

    /// <summary>
    /// The size of the object in grid units.
    /// </summary>
    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;

    /// <summary>
    /// The prefab associated with this object.
    /// </summary>
    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    /// <summary>
    /// The type of the object (e.g., ground, wall).
    /// </summary>
    [field: SerializeField]
    public ObjectType objectType { get; private set; }
    
    /// <summary>
    /// World resource name
    /// </summary>
    [field: SerializeField]
    public string worldResourceName { get; private set; }
    
    /// <summary>
    /// World resource name
    /// </summary>
    [field: SerializeField]
    public int cost { get; private set; }
}