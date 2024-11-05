using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractables : MonoBehaviour
{
    public static WorldInteractables instance; 
    
    public List<GameObject> ArcadeMachines { get; private set; } = new List<GameObject>();

    private void Awake()
    {
        instance = this;    
    }
}
