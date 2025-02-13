using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class ExpandButtonsUsabilitySetup : MonoBehaviour
{
    [SerializeField] private ExpandHandler expandHandler;

    [SerializeField] private Button[] buttons;

    MoneyManager moneyManager;

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = expandHandler.costs[i].ToString();
        }
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        moneyManager = MoneyManager.instance;
        
        for (int i = 0; i < buttons.Length; i++)
        {
            if (expandHandler.costs[i] <= moneyManager.MoneyAmount)
            {
                buttons[i].interactable = true;
            }
            else
            {
                buttons[i].interactable = false;
            }

            if (expandHandler.boughtLand[i])
            {
                buttons[i].interactable = false;
            }
        }
        
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = expandHandler.costs[i].ToString();
        }
    }
    
}
