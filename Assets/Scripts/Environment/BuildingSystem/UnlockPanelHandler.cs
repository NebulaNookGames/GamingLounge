using UnityEngine;
using UnityEngine.UI;

public class UnlockPanelHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private int[] cost;
    [SerializeField] private bool[] bought;
    [SerializeField] private Color validColor = Color.green;
    [SerializeField] private Color invalidColor = Color.red;
    [SerializeField] private Color unlockedColor = Color.blue;
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
        Debug.Log(amount);
        for(int i = 0; i < buttons.Length; i++)
        {
            if (bought[i])
            {
                buttons[i].GetComponent<Button>().interactable = false;
                buttons[i].GetComponent<Button>().image.color = unlockedColor;
                continue;
            }
            
            if (cost[i] <= amount)
            {
                buttons[i].GetComponent<Button>().interactable = true;
                buttons[i].GetComponent<Button>().image.color = validColor;
            }

            else
            {
                buttons[i].GetComponent<Button>().interactable = false;
                buttons[i].GetComponent<Button>().image.color = invalidColor;
            }
        }
    }

    public void UnlockItem(int index)
    {
        if (cost[index] > MoneyManager.instance.MoneyAmount) return;
       
        bought[index] = true;
        unlockables[index].SetActive(true);
        buttons[index].GetComponent<Button>().interactable = false;
        buttons[index].GetComponent<Button>().image.color = unlockedColor;
        MoneyManager.instance.ChangeMoney(-cost[index]);
    }

    public void UnlockAllItems()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            unlockables[i].SetActive(true);
            buttons[i].GetComponent<Button>().interactable = false;
            buttons[i].GetComponent<Button>().image.color = unlockedColor;
            bought[i] = true;
            
        }
        
        UpdateButtonInteractable(MoneyManager.instance.MoneyAmount);
    }
}