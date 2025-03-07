using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UnlockPanelHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private int[] cost;
    [SerializeField] public bool[] bought;
    [SerializeField] private GameObject[] unlockables;
    public UnityEvent onUpgradePCUnlocked;
    private bool firstBuy = true; 
    
    private void OnEnable()
    {
        MoneyManager.instance.OnMoneyChanged += UpdateButtonInteractable;
        UpdateButtonInteractable(MoneyManager.instance.MoneyAmount);
    }
    
    private void OnDisable()
    {
        MoneyManager.instance.OnMoneyChanged -= UpdateButtonInteractable;
    }

    void UpdateButtonInteractable(int amount)
    {
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
        if (cost[index] > MoneyManager.instance.MoneyAmount) return;


        if (firstBuy && !SaveAndLoad.instance.saveDataLoaded)
        {
            InputManager.instance.placementInputUnlocked = true;
            onUpgradePCUnlocked?.Invoke();
            if(SteamIntegration.instance && Steamworks.SteamClient.IsValid)
                SteamIntegration.instance.UnlockAchievement("UPGRADEPCUNLOCK");
        }
        firstBuy = false;  
        bought[index] = true;
        unlockables[index].SetActive(true);
        buttons[index].GetComponent<Button>().interactable = false;
        MoneyManager.instance.ChangeMoney(-cost[index]);
        
        if(SteamIntegration.instance && Steamworks.SteamClient.IsValid)
            UnlockAchievement(index);
    }

    public void UnlockAllItems()
    {
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
        for(int i = 0; i < this.bought.Length; i++)
        {
            if(this.bought[i]) 
                unlockables[i].SetActive(true);
        }
    }

    void UnlockAchievement(int index)
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
            case 18: // rock 
                SteamIntegration.instance.UnlockAchievement("ROCKUNLOCK");
                break;
        }
    }
}