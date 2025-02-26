using System;
using UnityEngine;

public class AddMoneyOnDestroy : MonoBehaviour
{
    int amount;
    bool shouldAddMoney;

    private void OnEnable()
    {
        shouldAddMoney = true;
    }

    public void SetAmount(int amount)
    {
        this.amount = amount;
    }
    
    private void OnDestroy()
    {
        // if(shouldAddMoney)
        //     MoneyManager.instance.ChangeMoney(amount);
    }
}
