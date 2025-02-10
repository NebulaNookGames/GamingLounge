using UnityEngine;
using System.Collections.Generic;

public class EntityManager : MonoBehaviour
{
    public static EntityManager instance; // Singleton instance of EntityManager
    public List<NPCValues> npcValues; // List to store NPC values
    public List<GameObject> currentNPCs; // List to keep track of current NPC GameObjects
    public EntitySpawner spawner; // Reference to the spawner for creating NPCs

    private void Awake()
    {
        // Initialize the singleton instance
        if(instance == null)
            instance = this;
    }

    public void DestroyNPC(GameObject npc)
    {
        npc.GetComponent<EffectSpawner>().SpawnEffect(); // Spawn effect before destroying NPC
        currentNPCs.Remove(npc); // Remove NPC from the current NPCs list
        npcValues.Remove(npc.GetComponent<NPCValueHolder>().values); // Remove associated values
        Destroy(npc); // Destroy the NPC GameObject
    }
}

[System.Serializable]
public class NPCValues
{
    public int randomIndex = 0; // Random identifier for the NPC
    public int colorIndex = 0; // Index to determine NPC's color
    public bool invitedToLounge = false; // Flag to indicate if NPC is invited to lounge
    public Vector3 lastLocation = Vector3.zero; // Last known location of the NPC

    public NPCValues(int randomIndex, int colorIndex, bool invitedToLounge)
    {
        this.randomIndex = randomIndex;
        this.colorIndex = colorIndex; 
    }
}
