using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnergyGeneration : MonoBehaviour
{
    public int upgradeAmount = 2;
    public float currentEnergy;
    public float maxEnergy;
    public float[] energyPerSecond; 
    public float[] upgradeEnergyAmounts;
    public int[] upgradeCost; 
    public int upgradeIndex;
    public GameObject interactionSymbol;
    private Transform player;
    public Transform objectToCheckDistanceFrom;
    public float interactionDistance;
    public InputActionProperty interactAction;
    private EnergyHandler[] energyHandlersNearby; 
    
    private void OnEnable()
    {
        maxEnergy = upgradeEnergyAmounts[upgradeIndex];
        player = GameObject.FindGameObjectWithTag("Player").transform;
        interactAction.action.performed += Interact;
    }
    
    private void OnDisable()
    {
        interactAction.action.performed -= Interact;
        foreach (EnergyHandler handler in energyHandlersNearby)
        {
            handler.energy = null; 
        }
    }

    void Interact(InputAction.CallbackContext context)
    {
        if (interactionSymbol.activeSelf)
        {
            UpgradeEnergy();
        }
    }

    private void Update()
    {
        float dist = Vector3.Distance(objectToCheckDistanceFrom.position, player.position);
        if (dist < interactionDistance)
        {
            interactionSymbol.SetActive(true);
        }
        else
        {
            interactionSymbol.SetActive(false);
        }
        
        
        if(currentEnergy < maxEnergy)
            currentEnergy += energyPerSecond[upgradeIndex] * Time.deltaTime;
        else
            currentEnergy = maxEnergy;
    }

    public bool EnergyAvailable(float amount)
    {
        if (currentEnergy >= amount)
            return true;
        
        return false; 
    }

    public float TakeEnergy(float amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            return amount;
        }
        
        return 0;
    }

    public void UpgradeEnergy()
    {
        if (upgradeIndex < upgradeAmount && MoneyManager.instance.MoneyAmount >= upgradeCost[upgradeIndex])
        {
            upgradeIndex++;
            maxEnergy = upgradeEnergyAmounts[upgradeIndex];
            MoneyManager.instance.ChangeMoney(-upgradeCost[upgradeIndex]);
        }
        else
        {
            Debug.Log("Upgrade not possible");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Machine"))
        {
            if(other.transform.root.GetComponentInChildren<EnergyHandler>())
                other.transform.root.GetComponentInChildren<EnergyHandler>().energy = this;
        }
    }
}
