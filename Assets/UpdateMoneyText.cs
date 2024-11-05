using System;
using UnityEngine;
using TMPro;

public class UpdateMoneyText : MonoBehaviour
{
    [SerializeField] private MoneyManager moneyManager;
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        moneyManager.OnMoneyChanged += UpdateText;
    }

    void UpdateText(int money)
    {
        text.text = money.ToString();
    }
}
