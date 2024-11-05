using UnityEngine;
using System;

public class MoneyHolder : MonoBehaviour
{
    public int moneyBeingHeld {get; private set;}
    
    public event Action<int> OnMoneyChanged;

    public void ChangeMoney(int amount)
    {
        moneyBeingHeld += amount;
        OnMoneyChanged?.Invoke(moneyBeingHeld);
    }
}
