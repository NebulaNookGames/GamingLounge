using System;
using TMPro;
using UnityEngine;

public class MultiplierTextUpdater : MonoBehaviour
{
    [SerializeField] private Cost cost;

    public TextMeshProUGUI text;

    
    /// <summary>
    /// Initializes the component and subscribes to the money change event.
    /// </summary>
    private void Awake()
    {
        cost.onMultiplierUpdated += UpdateText;
    }

    private void OnEnable()
    {
        cost.onMultiplierUpdated += UpdateText;
        UpdateText();
    }

    private void OnDisable()
    {
        cost.onMultiplierUpdated -= UpdateText;
    }

    void UpdateText()
    {
        if (text == null || !text.enabled) return;
        int multiplier = cost.multiplier;
        if(multiplier > 5)
            multiplier = 5;
        
        text.text = "+" + multiplier.ToString();
    }
}