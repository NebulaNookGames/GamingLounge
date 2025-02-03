using System;
using UnityEngine;
using System.Collections.Generic;

public class MachineIncomeMultiplier : MonoBehaviour
{
    [SerializeField] private float scanRadius = 5f; // The radius in which machines are affected
    [SerializeField] private LayerMask machineLayer; // Only detect machines
    public List<Cost> affectedMachines = new List<Cost>(); // Store affected machines

    public bool canAddToMultiplier = false;
    public Transform centerofObject; 
    public GameObject scanAreaPrefab;
    public GameObject scanAreaInstance; 
    public void ChangeCanAddToMultiplier()
    {
        canAddToMultiplier = true;
    }
    
    private void Start()
    {
        if (!canAddToMultiplier)
        {
            if (scanAreaPrefab != null)
            {
                scanAreaInstance = Instantiate(scanAreaPrefab, centerofObject.transform.position, Quaternion.identity);
                scanAreaInstance.transform.localScale = new Vector3(scanRadius * 2, 0.1f, scanRadius * 2); // Set scale based on radius
                scanAreaInstance.SetActive(true);
            }
        }
        else
        {
            FindAndStoreMachines();
        }
    }

    private void Update()
    {
        if(scanAreaInstance != null)
            scanAreaInstance.transform.position = new Vector3(centerofObject.transform.position.x, scanAreaInstance.transform.position.y, centerofObject.transform.position.z);
    }

    private void OnDestroy()
    {
        if(scanAreaInstance != null)
            Destroy(scanAreaInstance);
        
        if (!canAddToMultiplier) return;
        AdjustMultipliers(false);
    }

    private void FindAndStoreMachines()
    {
        Collider[] hitColliders = Physics.OverlapSphere(centerofObject.transform.position, scanRadius, machineLayer);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Machine"))
            {
                Transform obj; 
                obj = hitCollider.transform;
                while (obj.parent != null)
                {
                    obj = obj.parent;
                }
                Cost machine = obj.GetComponent<Cost>();
                if (machine != null && !affectedMachines.Contains(machine))
                {
                    affectedMachines.Add(machine);
                    machine.ChangeMultiplier(true);
                }
            }
        }
        
        Invoke(nameof(FindAndStoreMachines), 2f);
    }

    private void AdjustMultipliers(bool addMultiplier)
    {
        foreach (var machine in affectedMachines)
        {
            if (machine != null)
            {
                machine.ChangeMultiplier(addMultiplier);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(centerofObject.transform.position, scanRadius);
    }
}