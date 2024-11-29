using System;
using System.Linq;
using UnityEngine;

public class ActivatePlacedObject : MonoBehaviour
{
    [SerializeField] private Collider[] colliders;
    private AddMoneyOnDestroy addMoneyOnDestroy;
    
    private void OnEnable()
    {
        foreach (Collider col in colliders)
        { 
            col.enabled = false;
        }
        
        addMoneyOnDestroy = GetComponent<AddMoneyOnDestroy>();
    }

    public void Enable()
    {
        foreach (Collider col in colliders)
        { 
            col.enabled = true;
        }
        
        addMoneyOnDestroy.enabled = true; 
    }
}