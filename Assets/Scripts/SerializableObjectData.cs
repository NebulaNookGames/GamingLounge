using UnityEngine; 

[System.Serializable]
public class SerializableObjectData
{
    public string Name;
    public int ID;
    public SerializableVector2Int Size;
    public string PrefabName; // Store prefab name instead of GameObject
    public ObjectType objectType;
    public string worldResourceName;
    public int cost;

    public bool shouldCheckForOverlap;
    public int overlapCheckingLayerMask;

    public bool shouldCheckForDatabase;
    public ObjectType[] collisionObjectTypes;

    public bool shouldCheckForAllowCollisions;
    public ObjectType[] allowCollisions;

    // Conversion methods go below
    
    public static SerializableObjectData FromObjectData(ObjectData obj)
    {
        return new SerializableObjectData
        {
            Name = obj.Name,
            ID = obj.ID,
            Size = new SerializableVector2Int(obj.Size),
            PrefabName = obj.Prefab != null ? obj.Prefab.name : string.Empty,
            objectType = obj.objectType,
            worldResourceName = obj.worldResourceName,
            cost = obj.cost,

            shouldCheckForOverlap = obj.shouldCheckForOverlap,
            overlapCheckingLayerMask = obj.OverlapCheckingLayermask.value,  // <-- fix here

            shouldCheckForDatabase = obj.shouldCheckForDatabase,
            collisionObjectTypes = obj.collisionObjectTypes,

            shouldCheckForAllowCollisions = obj.shouldCheckForAllowCollisions,
            allowCollisions = obj.allowCollisions
        };
    }
    
    public ObjectData ToObjectData()
    {
        GameObject loadedPrefab = !string.IsNullOrEmpty(PrefabName) ? Resources.Load<GameObject>(PrefabName) : null;

        return new ObjectData(
            Name,
            ID,
            Size.ToVector2Int(),
            loadedPrefab,
            objectType,
            worldResourceName,
            cost,
            shouldCheckForOverlap,
            new LayerMask { value = overlapCheckingLayerMask },
            shouldCheckForDatabase,
            collisionObjectTypes,
            shouldCheckForAllowCollisions,
            allowCollisions
        );
    }
}