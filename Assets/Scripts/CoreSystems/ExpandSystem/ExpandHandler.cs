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
    public int addAmountPerLand = 5000;

    public void BuyLand(int landIndex)
    {
        moneyManager = MoneyManager.instance;
        
        if (costs[landIndex] <= moneyManager.MoneyAmount && !boughtLand[landIndex])
        {
            placementSystem.unlockedGridParts.Add(lands[landIndex]);
            boughtLand[landIndex] = true;
            moneyManager.ChangeMoney(-costs[landIndex]);
        }

        for(int i = 0; i < costs.Length; i++)
        {
            costs[i] += addAmountPerLand;
        }
        
        expandButtonsUsabilitySetup.Refresh();
    }

    public void UpdateBoughtLand(bool[] boughtLand)
    {
        this.boughtLand = boughtLand;
        for (int i = 0; i < lands.Length; i++)
        {
            if (this.boughtLand[i])
            {
                placementSystem.unlockedGridParts.Add(lands[i]);
                for(int j = 0; j < costs.Length; j++)
                {
                    costs[j] += addAmountPerLand;
                }
            }
        }
      
        expandButtonsUsabilitySetup.Refresh();
    }
}
