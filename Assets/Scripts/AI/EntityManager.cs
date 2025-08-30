using UnityEngine;
using System.Collections.Generic;
using System; 

/// <summary>
/// Manages the creation, destruction, and tracking of NPC entities in the game.
/// </summary>
public class EntityManager : MonoBehaviour
{
    #region Variables

    [Header("Entity Management")]
    [Tooltip("Singleton instance of the EntityManager.")]
    public static EntityManager instance; // Singleton instance of EntityManager

    [Tooltip("List to store NPC values.")]
    public List<NPCValues> npcValues; // List to store NPC values

    [Tooltip("List to keep track of current NPC GameObjects.")]
    public List<GameObject> currentNPCs; // List to keep track of current NPC GameObjects

    [Tooltip("Reference to the spawner responsible for creating NPCs.")]
    public EntitySpawner spawner; // Reference to the spawner for creating NPCs

    #endregion Variables

    #region Unity Methods

    /// <summary>
    /// Called when the script is first loaded. Initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
            instance = this; // Ensure only one instance of EntityManager exists
    }

    #endregion Unity Methods

    #region Methods

    /// <summary>
    /// Destroys an NPC and spawns a visual effect before doing so.
    /// </summary>
    /// <param name="npc">The NPC GameObject to be destroyed.</param>
    public void DestroyNPC(GameObject npc)
    {
        // Spawn visual effect before NPC destruction
        npc.GetComponent<EffectSpawner>().SpawnEffect(); 

        // Remove NPC from the current list and NPC values list
        currentNPCs.Remove(npc); 
        npcValues.Remove(npc.GetComponent<NPCValueHolder>().Values); 

        // Destroy the NPC GameObject
        Destroy(npc); 
    }

    #endregion Methods
}

/// <summary>
/// Stores values and properties related to an NPC in the game.
/// </summary>
[Serializable]
public class NPCValues
{
    [Header("NPC Properties")]

    [Tooltip("Random identifier for the NPC.")]
    public int randomIndex = 0;

    [Tooltip("Index to determine NPC's color.")]
    public int colorIndex = 0;

    [Tooltip("Flag to indicate if NPC is invited to lounge.")]
    public bool invitedToLounge = false;

    // Unity field (not binary-serializable)
    [NonSerialized]
    public Vector3 lastLocation = Vector3.zero;

    // Serializable proxy field for binary serialization
    public SerializableVector3 lastLocationSerialized;

    #region Constructors

    /// <summary>
    /// Default constructor for serialization.
    /// </summary>
    public NPCValues() { }

    /// <summary>
    /// Constructor to initialize NPC values.
    /// </summary>
    public NPCValues(int randomIndex, int colorIndex, bool invitedToLounge)
    {
        this.randomIndex = randomIndex;
        this.colorIndex = colorIndex;
        this.invitedToLounge = invitedToLounge;
    }

    #endregion

    #region Binary Serialization Helpers

    /// <summary>
    /// Converts non-serializable fields to their serializable counterparts.
    /// </summary>
    public void PrepareForBinarySave()
    {
        lastLocationSerialized = new SerializableVector3(lastLocation);
    }

    /// <summary>
    /// Converts serializable fields back into Unity-native values.
    /// </summary>
    public void RestoreFromBinaryLoad()
    {
        lastLocation = lastLocationSerialized.ToVector3();
    }

    #endregion
}