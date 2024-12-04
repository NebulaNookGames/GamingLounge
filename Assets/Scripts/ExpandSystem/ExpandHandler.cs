using System;
using System.Collections.Generic;
using UnityEngine;

public class ExpandHandler : MonoBehaviour
{
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private GameObject[] lands;
    [SerializeField] public int[] costs;
    [SerializeField] public bool[] boughtLand;
    [SerializeField] public ObjectActivator objectActivator;
    MoneyManager moneyManager; 

    public void BuyLand(int landIndex)
    {
        moneyManager = MoneyManager.instance;
        
        if (costs[landIndex] <= moneyManager.MoneyAmount && !boughtLand[landIndex])
        {
            placementSystem.unlockedGridParts.Add(lands[landIndex]);
            boughtLand[landIndex] = true;
            objectActivator.OnDisable();
            moneyManager.ChangeMoney(-costs[landIndex]);
        }
        
    }
}
