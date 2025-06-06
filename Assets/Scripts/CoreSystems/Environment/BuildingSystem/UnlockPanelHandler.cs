using System;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UnlockPanelHandler : MonoBehaviour
{
    public int unlockSize = 26; 
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private int[] cost;
    [SerializeField] public bool[] bought;
    [SerializeField] private GameObject[] unlockables;
    public UnityEvent onUpgradePCUnlocked;
    private bool firstBuy = true;
    IEnumerator UnlockAchievementsAfterLoad()
    {
        CheckBoughtSize();
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < bought.Length; i++)
        {
            if (bought[i])
            {
                UnlockAchievement(i);
                buttons[i].GetComponent<ChangedButtonDisabledColor>().ChangeButtonDisabledColor();
            }
        }
    }

    private void OnEnable()
    {
        CheckBoughtSize();
        MoneyManager.instance.OnMoneyChanged += UpdateButtonInteractable;
        UpdateButtonInteractable(MoneyManager.instance.MoneyAmount);
        StartCoroutine(UnlockAchievementsAfterLoad());
    }
    
    private void OnDisable()
    {
        MoneyManager.instance.OnMoneyChanged -= UpdateButtonInteractable;
    }

    void UpdateButtonInteractable(int amount)
    {
        CheckBoughtSize();
        
        for(int i = 0; i < buttons.Length; i++)
        {
            if (bought[i])
            {
                buttons[i].GetComponent<Button>().interactable = false;
                continue;
            }
            
            if (cost[i] <= amount)
            {
                buttons[i].GetComponent<Button>().interactable = true;
            }

            else
            {
                buttons[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    public void UnlockItem(int index)
    {
        CheckBoughtSize();
        
        if (cost[index] > MoneyManager.instance.MoneyAmount) return;


        if (firstBuy && !SaveAndLoad.instance.saveDataLoaded)
        {
            InputManager.instance.placementInputUnlocked = true;
            onUpgradePCUnlocked?.Invoke();
#if !UNITY_SWITCH
            try
            {
                SteamIntegration.instance.UnlockAchievement("UPGRADEPCUNLOCK");
            }
            catch
            {
               Debug.Log("Failed to unlock upgrade achievement");
            }
#endif
        }
        firstBuy = false;  
        bought[index] = true;
        unlockables[index].SetActive(true);
        buttons[index].GetComponent<Button>().interactable = false;
        MoneyManager.instance.ChangeMoney(-cost[index]);
        UnlockAchievement(index);
    }

    public void UnlockAllItems()
    {
        CheckBoughtSize();
        
        for (int i = 0; i < buttons.Length; i++)
        {
            unlockables[i].SetActive(true);
            buttons[i].GetComponent<Button>().interactable = false;
            bought[i] = true;
            
        }
        
        UpdateButtonInteractable(MoneyManager.instance.MoneyAmount);
    }

    public void LoadUnlocks(bool[] bought)
    {
        this.bought = bought;
        CheckBoughtSize();
        for(int i = 0; i < this.bought.Length; i++)
        {
            if (this.bought[i])
            {
                unlockables[i].SetActive(true);
            }
        }
    }

    void CheckBoughtSize()
    {
        if (bought.Length < unlockSize)
        {
            bool[] temp = bought;
            bought = new bool[unlockSize];
            for (int i = 0; i < temp.Length && i < unlockSize; i++)
            {
                bought[i] = temp[i];
            }
        }
    }

    void UnlockAchievement(int index)
    {
#if !UNITY_SWITCH
        try
        {
            switch (index)
            {
                case 0: // Tier 2
                    SteamIntegration.instance.UnlockAchievement("TIER2STRUCTUREUNLOCK");
                    break;
                case 1: // Tier 3
                    SteamIntegration.instance.UnlockAchievement("TIER3STRUCTUREUNLOCK");
                    break;
                case 2: // Crystal
                    SteamIntegration.instance.UnlockAchievement("CRYSTALUNLOCK");
                    break;
                case 3: // lightsaber
                    SteamIntegration.instance.UnlockAchievement("LIGHTSABERUNLOCK");
                    break;
                case 4: // flux
                    SteamIntegration.instance.UnlockAchievement("FLUXCAPACITORUNLOCK");
                    break;
                case 5: // traffic cone
                    SteamIntegration.instance.UnlockAchievement("TRAFFICCONEUNLOCK");
                    break;
                case 6: // couch
                    SteamIntegration.instance.UnlockAchievement("COUCHUNLOCK");
                    break;
                case 7: // trophy
                    SteamIntegration.instance.UnlockAchievement("TROPHYUNLOCK");
                    break;
                case 8: // upgrade pc
                    SteamIntegration.instance.UnlockAchievement("UPGRADEPCUNLOCK");
                    break;
                case 9: // arcade machine
                    SteamIntegration.instance.UnlockAchievement("ARCADEMACHINEUNLOCK");
                    break;
                case 10: // bike machine
                    SteamIntegration.instance.UnlockAchievement("BIKEMACHINEUNLOCK");
                    break;
                case 11: // racing machine
                    SteamIntegration.instance.UnlockAchievement("RACINGMACHINEUNLOCK");
                    break;
                case 12: // plant
                    SteamIntegration.instance.UnlockAchievement("PLANTUNLOCK");
                    break;
                case 13: // chair
                    SteamIntegration.instance.UnlockAchievement("CHAIRUNLOCK");
                    break;
                case 14: // nature spring
                    SteamIntegration.instance.UnlockAchievement("SPRINGNATUREUNLOCK");
                    break;
                case 15: // nature fall
                    SteamIntegration.instance.UnlockAchievement("FALLNATUREUNLOCK");
                    break;
                case 16: // nature candyland
                    SteamIntegration.instance.UnlockAchievement("CANDYLANDNATUREUNLOCK");
                    break;
                case 17: // Lounge Chair 
                    SteamIntegration.instance.UnlockAchievement("LOUNGECHAIRUNLOCK");
                    break;
                case 18: // rock 
                    SteamIntegration.instance.UnlockAchievement("ROCKUNLOCK");
                    break;
                case 19: // Tier4 
                    SteamIntegration.instance.UnlockAchievement("TIER4STRUCTUREUNLOCK");
                    break;
                case 20: // Satellite dish
                    SteamIntegration.instance.UnlockAchievement("SATELLITEDISHUNLOCK");
                    break;
                case 21: // Hologram
                    SteamIntegration.instance.UnlockAchievement("HOLOGRAMUNLOCK");
                    break;
                case 22: // Couch02
                    SteamIntegration.instance.UnlockAchievement("SCIFICOUCHUNLOCK");
                    break;
                case 23: // Fence
                    SteamIntegration.instance.UnlockAchievement("FENCEUNLOCK");
                    break;
                case 24: // Floating lamp
                    SteamIntegration.instance.UnlockAchievement("FLOATINGLAMPUNLOCK");
                    break;
                case 25: // Square lamp
                    SteamIntegration.instance.UnlockAchievement("LAMPUNLOCK");
                    break;
            }
        }
        catch
        {
            Debug.Log("Failed unlock achievement");
        }
#endif 
    }
}