using System;
using UnityEngine;

public class EnergyHandler : MonoBehaviour
{
    public EnergyGeneration energy;

    public float energyConsumption;
    public float energyConsumptionDuration;
    private float currentEnergyConsumptionDuration;
    public GameObject noEnergyIndicator;
    private bool madeNotAvailable;
    private void Awake()
    {
        currentEnergyConsumptionDuration = energyConsumptionDuration;
    }

    private void Update()
    {
        if (energy == null)
        {
            if (WorldInteractables.instance.availableArcadeMachines.Contains(transform.parent.gameObject))
            {
                WorldInteractables.instance.availableArcadeMachines.Remove(transform.parent.gameObject);
                noEnergyIndicator.SetActive(true);
                madeNotAvailable = true; 
            }
            
            Debug.Log("No Generator nearby");
            // Disable Machine
            return; 
        }
        
        if(energy && madeNotAvailable && energy.EnergyAvailable(energyConsumption))
        {
            if (!WorldInteractables.instance.availableArcadeMachines.Contains(transform.parent.gameObject))
            {
                WorldInteractables.instance.availableArcadeMachines.Add(transform.parent.gameObject);
                noEnergyIndicator.SetActive(false);
                madeNotAvailable = false; 
            }
        }
        
        currentEnergyConsumptionDuration -= Time.deltaTime;
        if (currentEnergyConsumptionDuration <= 0)
        {
            currentEnergyConsumptionDuration = energyConsumptionDuration;

            if (energy.EnergyAvailable(energyConsumption))
                energy.TakeEnergy(energyConsumption);

            else
            {
                if (WorldInteractables.instance.availableArcadeMachines.Contains(transform.parent.gameObject))
                {
                    WorldInteractables.instance.availableArcadeMachines.Remove(transform.parent.gameObject);
                    noEnergyIndicator.SetActive(true);
                    madeNotAvailable = true; 
                }
                Debug.Log("No energy available");
                // Disable Machine
            }
        }
    }
}