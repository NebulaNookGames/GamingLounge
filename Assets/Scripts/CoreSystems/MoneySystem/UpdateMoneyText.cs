using System;
using UnityEngine;
using TMPro;

public class UpdateMoneyText : MonoBehaviour
{
    public bool makeInvisible = true;
    public bool updateOnEnable = false; 
    
    [SerializeField] private MoneyManager moneyManager;

    private TextMeshProUGUI text;
    public GameObject background;
    public GameObject image;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (moneyManager != null)
        {
            moneyManager.OnMoneyChanged += UpdateText;
            
            if (updateOnEnable)
                UpdateText(moneyManager.MoneyAmount);
        }
    }

    private void OnDisable()
    {
        if (moneyManager != null)
            moneyManager.OnMoneyChanged -= UpdateText;

        if (IsInvoking(nameof(DisableMoneyDisplay)))
            CancelInvoke(nameof(DisableMoneyDisplay));
    }

    private void OnDestroy()
    {
        if (moneyManager != null)
            moneyManager.OnMoneyChanged -= UpdateText;

        if (IsInvoking(nameof(DisableMoneyDisplay)))
            CancelInvoke(nameof(DisableMoneyDisplay));
    }

    void UpdateText(int money)
    {
        if (this == null) return; // safety: object destroyed

        if (text != null)
        {
            text.enabled = true;
            text.text = money.ToString();
        }
        if (background != null)
            background.SetActive(true);
        
        if (image != null)
            image.SetActive(true);

        if (makeInvisible)
        {
            if (IsInvoking(nameof(DisableMoneyDisplay)))
                CancelInvoke(nameof(DisableMoneyDisplay));

            Invoke(nameof(DisableMoneyDisplay), 5f);
        }
    }

    void DisableMoneyDisplay()
    {
        if (this == null) return; // object destroyed

        if (background != null)
            background.SetActive(false);
        
        if (image != null)
            image.SetActive(false);
        
        if (text != null)
            text.enabled = false;
    }
}
