using UnityEngine;
using System;

/// <summary>
/// This class manages the player's money and triggers events when the money changes.
/// It allows other components to react to changes in the money amount.
/// </summary>
public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    [SerializeField] private int moneyAmount = 5000;
    // The current amount of money the player has.
    public int MoneyAmount
    {
        get { return moneyAmount;}
    private set { moneyAmount = value; } }

    // Event triggered when the money amount changes.
    public event Action<int> OnMoneyChanged;

    private void Awake()
    {
        instance = this; 
    }

    public void SetMoney(int amount)
    {
        moneyAmount = amount;
        
        if (moneyAmount > 99999999)
            moneyAmount = 99999999;
        
        OnMoneyChanged?.Invoke(moneyAmount);
    }

    /// <summary>
    /// Changes the money amount by a specified amount and triggers the OnMoneyChanged event.
    /// </summary>
    /// <param name="amount">The amount to change the money by.</param>
    public void ChangeMoney(int amount)
    {
        moneyAmount += amount; // Update the money amount.
        
        if (moneyAmount > 99999999)
            moneyAmount = 99999999;
        
        OnMoneyChanged?.Invoke(moneyAmount); // Trigger the OnMoneyChanged event.
    }
}