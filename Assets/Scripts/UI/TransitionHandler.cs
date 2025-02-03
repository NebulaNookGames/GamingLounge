using System;
using UnityEngine;

public class TransitionHandler : MonoBehaviour
{
    public static TransitionHandler instance;
    
    
    
    
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
        }
        
        
        DontDestroyOnLoad(gameObject);
    }
}
