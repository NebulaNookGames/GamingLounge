using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HandleCostChecking : MonoBehaviour
{
    [SerializeField] private ObjectsDatabaseSO dataBase;

    [SerializeField] private GameObject[] buttons;
    [SerializeField] private int[] indexes;
    
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
            if (dataBase.objectsData[indexes[i]].cost <= amount)
                buttons[i].GetComponent<Button>().interactable = true;
            else
                buttons[i].GetComponent<Button>().interactable = false;
        }
    }
}