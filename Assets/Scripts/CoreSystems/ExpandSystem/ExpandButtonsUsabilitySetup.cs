using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class ExpandButtonsUsabilitySetup : MonoBehaviour
{
    [SerializeField] private Color validColor = Color.green;

    [SerializeField] private Color invalidColor = Color.red;

    [SerializeField] private Color alreadyUnlockedColor = Color.blue; 
    
    [SerializeField] private ExpandHandler expandHandler;

    [SerializeField] private Button[] buttons;

    MoneyManager moneyManager;

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text =
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text + " " + expandHandler.costs[i];
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
                buttons[i].image.color = validColor;
            }
            else
            {
                buttons[i].interactable = false;
                buttons[i].image.color = invalidColor;
            }

            if (expandHandler.boughtLand[i])
            {
                buttons[i].interactable = false;
                buttons[i].image.color = alreadyUnlockedColor;
            }
        }
    }
}
