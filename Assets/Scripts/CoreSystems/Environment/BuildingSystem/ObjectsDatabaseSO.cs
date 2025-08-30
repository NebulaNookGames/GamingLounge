using UnityEngine;
using System.Collections.Generic;
using System; 

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

[Serializable]
public class ObjectData
{
    [Header("General Settings")]

    [Tooltip("The name of the object")]
    [field: SerializeField] public string Name { get; private set; }

    [Tooltip("The unique ID of the object.")]
    [field: SerializeField] public int ID { get; private set; }

    // Original Unity field, serialized by Unity/JSON
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

    [SerializeField] private LayerMask overlapCheckingLayermask;
    public LayerMask OverlapCheckingLayermask => overlapCheckingLayermask;

    [SerializeField] private int overlapCheckingLayermaskValue;

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

    // Proxy field for binary serialization of Size
    public SerializableVector2Int sizeSerializable;

    public ObjectData(
        string name,
        int id,
        Vector2Int size,
        GameObject prefab,
        ObjectType objectType,
        string worldResourceName,
        int cost,
        bool shouldCheckForOverlap,
        LayerMask overlapCheckingLayermask,
        bool shouldCheckForDatabase,
        ObjectType[] collisionObjectTypes,
        bool shouldCheckForAllowCollisions,
        ObjectType[] allowCollisions)
    {
        Name = name;
        ID = id;
        Size = size;
        Prefab = prefab;
        this.objectType = objectType;
        this.worldResourceName = worldResourceName;
        this.cost = cost;

        this.shouldCheckForOverlap = shouldCheckForOverlap;
        this.overlapCheckingLayermask = overlapCheckingLayermask;

        this.shouldCheckForDatabase = shouldCheckForDatabase;
        this.collisionObjectTypes = collisionObjectTypes;

        this.shouldCheckForAllowCollisions = shouldCheckForAllowCollisions;
        this.allowCollisions = allowCollisions;
    }
    
    /// <summary>
    /// Call before saving to binary format to fill proxy serializable fields.
    /// </summary>
    public void PrepareForBinarySave()
    {
        sizeSerializable = new SerializableVector2Int(Size);
        overlapCheckingLayermaskValue = overlapCheckingLayermask.value;
    }

    /// <summary>
    /// Call after loading from binary format to restore Unity fields.
    /// </summary>
    public void RestoreFromBinaryLoad()
    {
        Size = sizeSerializable.ToVector2Int();
        overlapCheckingLayermask = new LayerMask { value = overlapCheckingLayermaskValue };

        if (!string.IsNullOrEmpty(worldResourceName))
        {
            Prefab = Resources.Load<GameObject>(worldResourceName);
        }
        else
        {
            Prefab = null;
        }
    }
}