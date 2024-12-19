using System;
using System.Collections.Generic;
using UnityEngine;

public class ExpandHandler : MonoBehaviour
{
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private GameObject[] lands;
    [SerializeField] public int[] costs;
    [SerializeField] public bool[] boughtLand;

    [SerializeField] private ExpandButtonsUsabilitySetup expandButtonsUsabilitySetup;
    MoneyManager moneyManager; 

    public void BuyLand(int landIndex)
    {
        moneyManager = MoneyManager.instance;
        
        if (costs[landIndex] <= moneyManager.MoneyAmount && !boughtLand[landIndex])
        {
            placementSystem.unlockedGridParts.Add(lands[landIndex]);
            boughtLand[landIndex] = true;
            moneyManager.ChangeMoney(-costs[landIndex]);
        }
        
        expandButtonsUsabilitySetup.Refresh();
    }

    public void UpdateBoughtLand(bool[] boughtLand)
    {
        this.boughtLand = boughtLand;
        for (int i = 0; i < lands.Length; i++)
        {
            if(this.boughtLand[i])
                placementSystem.unlockedGridParts.Add(lands[i]);
        }
        expandButtonsUsabilitySetup.Refresh();
    }
}
