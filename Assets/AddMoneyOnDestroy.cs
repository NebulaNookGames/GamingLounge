using System;
using UnityEngine;

public class AddMoneyOnDestroy : MonoBehaviour
{
    public int amount;
    public bool shouldAddMoney;

    private void OnEnable()
    {
        shouldAddMoney = true;
    }

    private void OnDestroy()
    {
        if(shouldAddMoney)
            MoneyManager.instance.ChangeMoney(amount);
    }
}
