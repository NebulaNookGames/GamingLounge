using System;
using UnityEngine;
using TMPro;

/// <summary>
/// This class updates the displayed money value in the UI whenever the amount of money changes.
/// It listens for the money change event from the MoneyManager and updates the text accordingly.
/// </summary>
public class UpdateMoneyText : MonoBehaviour
{

    public bool makeInvisible = true;

    public bool updateOnEnable = false; 
    
    // Reference to the MoneyManager that tracks and triggers money changes.
    [SerializeField] private MoneyManager moneyManager;

    // TextMeshProUGUI component used to display the money value on the UI.
    private TextMeshProUGUI text;

    public GameObject background;
    public GameObject image;

    /// <summary>
    /// Initializes the component and subscribes to the money change event.
    /// </summary>
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>(); // Get the TextMeshProUGUI component.
        moneyManager.OnMoneyChanged += UpdateText; // Subscribe to money change event.
    }

    private void OnEnable()
    {
        if(updateOnEnable)
            UpdateText(moneyManager.MoneyAmount);
    }

    private void OnDisable()
    {
        moneyManager.OnMoneyChanged -= UpdateText; // Subscribe to money change event.
        if (IsInvoking(nameof(DisableMoneyDisplay)))
            CancelInvoke((nameof(DisableMoneyDisplay)));
    }

    private void OnDestroy()
    {
        if (IsInvoking(nameof(DisableMoneyDisplay)))
            CancelInvoke((nameof(DisableMoneyDisplay)));
    }

    /// <summary>
    /// Updates the displayed money text when the money value changes.
    /// </summary>
    /// <param name="money">The new amount of money to display.</param>
    void UpdateText(int money)
    {
        if (text)
        {
            text.enabled = true;
            text.text = money.ToString(); // Update the money text.
        }
        if(background)
            background.SetActive(true);
        
        if(image)
            image.SetActive(true);

        if (makeInvisible)
        {
            if (IsInvoking(nameof(DisableMoneyDisplay)))
                CancelInvoke((nameof(DisableMoneyDisplay)));

            Invoke(nameof(DisableMoneyDisplay), 5f);
        }
    }

    void DisableMoneyDisplay()
    {
        if(background)
            background.SetActive(false);
        
        if(image)
            image.SetActive(false);
        
        if (text)
            text.enabled = false;
    }
}