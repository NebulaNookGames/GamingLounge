using UnityEngine;
using TMPro;

public class MoneyHolderTextUpdater : MonoBehaviour
{
    [SerializeField] private MoneyHolder moneyHolder;
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        moneyHolder.OnMoneyChanged += UpdateText;
    }

    void UpdateText(int money)
    {
        text.text = money.ToString();
    }
}
