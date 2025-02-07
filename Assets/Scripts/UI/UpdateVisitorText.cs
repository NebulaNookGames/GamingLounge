using System;
using UnityEngine;
using TMPro;

public class UpdateVisitorText : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void OnEnable()
    {
        EntitySpawner.instance.onAmountUpdated += UpdateText;
        text.text = EntitySpawner.instance.amount + "/" + EntitySpawner.instance.maxAmountFromLand;

    }

    private void OnDisable()
    {
        EntitySpawner.instance.onAmountUpdated -= UpdateText;
    }

    void UpdateText()
    {
        text.text = EntitySpawner.instance.amount + "/" + EntitySpawner.instance.maxAmountFromLand;
    }
}
