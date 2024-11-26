using System;
using UnityEngine;

public class EnableColliders : MonoBehaviour
{
    [SerializeField] private Collider[] colliders;
    AddMoneyOnDestroy addMoney;
    
    private void Awake()
    {
        if (GetComponent<AddMoneyOnDestroy>())
            addMoney = GetComponent<AddMoneyOnDestroy>();
    }

    private void OnEnable()
    {
        foreach (Collider col in colliders)
        { 
            col.enabled = false;
        }
    }

    public void Enable()
    {
        foreach (Collider col in colliders)
        { 
            col.enabled = true;
        }
        addMoney.enabled = true;

    }
}