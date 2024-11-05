using UnityEngine;
using System;

public class MoneyManager : MonoBehaviour
{
    public int moneyAmount { get; private set; }

    public event Action<int> OnMoneyChanged;

    public void ChangeMoney(int amount)
    {
        moneyAmount += amount;
        OnMoneyChanged?.Invoke(moneyAmount);
    }
}
