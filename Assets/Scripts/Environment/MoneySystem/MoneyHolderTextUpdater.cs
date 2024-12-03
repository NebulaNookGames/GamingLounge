using System;
using UnityEngine;
using TMPro;

/// <summary>
/// This class updates the displayed money value in the UI whenever the amount of money changes in the MoneyHolder.
/// It listens for the money change event and updates the text accordingly.
/// </summary>
public class MoneyHolderTextUpdater : MonoBehaviour
{
    // Reference to the MoneyHolder that triggers money changes.
    [SerializeField] private MoneyHolder moneyHolder;

    // TextMeshProUGUI component used to display the money value on the UI.
    public TextMeshProUGUI text;

    /// <summary>
    /// Initializes the component and subscribes to the money change event.
    /// </summary>
    private void Awake()
    {
        moneyHolder.OnMoneyChanged += UpdateText; // Subscribe to money change event.
    }

    private void OnEnable()
    {
        moneyHolder.OnMoneyChanged += UpdateText; // Subscribe to money change event.
    }

    private void OnDisable()
    {
        moneyHolder.OnMoneyChanged -= UpdateText; // Subscribe to money change event.
    }

    /// <summary>
    /// Updates the displayed money text when the money value changes.
    /// </summary>
    /// <param name="money">The new amount of money to display.</param>
    void UpdateText(int money)
    {
        if (text == null) return; 
        
        if(text.enabled)
            text.text = money.ToString(); // Update the money text.
    }
}