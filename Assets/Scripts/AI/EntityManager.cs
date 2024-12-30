using System;
using UnityEngine;
using System.Collections.Generic;

public class EntityManager : MonoBehaviour
{
    public static EntityManager instance;
    public List<NPCValues> npcValues;
    public EntitySpawner spawner; 
    
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }
}

[System.Serializable]
public class NPCValues
{
    public int randomIndex = 0;
    public int colorIndex = 0;
    
    public NPCValues(int randomIndex, int colorIndex)
    {
        this.randomIndex = randomIndex;
        this.colorIndex = colorIndex; 
    }
}
