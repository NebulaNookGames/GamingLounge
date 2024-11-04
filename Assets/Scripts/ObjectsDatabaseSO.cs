using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum representing the different types of objects that can be used in the game.
/// </summary>
public enum ObjectType 
{ 
    /// <summary> Represents ground objects that make up the base layer. </summary>
    Ground,
    
    /// <summary> Represents walls in the environment. </summary>
    Wall,
    
    /// <summary> Represents decorative objects that can be attached to walls. </summary>
    WallDecor,
    
    /// <summary> Represents furniture objects that can be placed in the environment. </summary>
    Furniture 
};

/// <summary>
/// A ScriptableObject database that holds a list of game objects that can be placed in the environment.
/// </summary>
[CreateAssetMenu(menuName = "Database/Objects Database")]
public class ObjectsDatabaseSO : ScriptableObject
{
    /// <summary>
    /// List of all object data entries available in the game.
    /// </summary>
    public List<ObjectData> objectsData;
}

/// <summary>
/// Serializable class holding data about a specific object, including its name, ID, size, prefab, and type.
/// </summary>
[Serializable]
public class ObjectData
{
    /// <summary>
    /// The name of the object as displayed in the game.
    /// </summary>
    [field: SerializeField]
    public string Name { get; private set; }

    /// <summary>
    /// A unique identifier for the object.
    /// </summary>
    [field: SerializeField]
    public int ID { get; private set; }

    /// <summary>
    /// The size of the object on the grid, defaulting to 1x1.
    /// </summary>
    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;

    /// <summary>
    /// The prefab associated with this object, which represents its appearance in the game.
    /// </summary>
    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    /// <summary>
    /// The type of the object, categorized by the <see cref="ObjectType"/> enum.
    /// </summary>
    [field: SerializeField]
    public ObjectType objectType { get; private set; }
}
