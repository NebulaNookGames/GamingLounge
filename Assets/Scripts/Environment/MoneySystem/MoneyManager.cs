using UnityEngine;
using System;

/// <summary>
/// This class manages the player's money and triggers events when the money changes.
/// It allows other components to react to changes in the money amount.
/// </summary>
public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance; 
    
    // The current amount of money the player has.
    [SerializeField] int moneyAmount = 50; 
    public int MoneyAmount {
        get { return moneyAmount; } private set {moneyAmount = value; } }

    // Event triggered when the money amount changes.
    public event Action<int> OnMoneyChanged;

    private void Awake()
    {
        instance = this; 
    }

    /// <summary>
    /// Changes the money amount by a specified amount and triggers the OnMoneyChanged event.
    /// </summary>
    /// <param name="amount">The amount to change the money by.</param>
    public void ChangeMoney(int amount)
    {
        MoneyAmount += amount; // Update the money amount.
        OnMoneyChanged?.Invoke(moneyAmount); // Trigger the OnMoneyChanged event.
    }
}