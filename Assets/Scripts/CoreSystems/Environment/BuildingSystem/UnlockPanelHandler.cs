using UnityEngine;
using UnityEngine.UI;

public class UnlockPanelHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private int[] cost;
    [SerializeField] public bool[] bought;
    [SerializeField] private GameObject[] unlockables;
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
       
        bought[index] = true;
        unlockables[index].SetActive(true);
        buttons[index].GetComponent<Button>().interactable = false;
        MoneyManager.instance.ChangeMoney(-cost[index]);
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
}