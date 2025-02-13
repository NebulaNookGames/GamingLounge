using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandleCostChecking : MonoBehaviour
{
    [SerializeField] private ObjectsDatabaseSO dataBase;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private int[] indexes;
    [SerializeField] private ObjectPlacer objectPlacer;
    public bool checkForUpgradePC = true; 
    
    private void OnEnable()
    {
        if (MoneyManager.instance != null)
        {
            MoneyManager.instance.OnMoneyChanged += UpdateButtonInteractable;
            UpdateButtonInteractable(MoneyManager.instance.MoneyAmount);
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            if(buttons[i].GetComponentInChildren<TextMeshProUGUI>())
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = dataBase.objectsData[indexes[i]].cost.ToString();
        }
    }

    private void OnDisable()
    {
        MoneyManager.instance.OnMoneyChanged -= UpdateButtonInteractable;
    }

    void UpdateButtonInteractable(int amount)
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if (dataBase.objectsData[indexes[i]].cost <= amount)
            {
                buttons[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                buttons[i].GetComponent<Button>().interactable = false; }
            
            if (i == 25 && objectPlacer.upgradePCIsPlaced && checkForUpgradePC)
            {
                buttons[i].GetComponent<Button>().interactable = false;
            }
        }
    }
}