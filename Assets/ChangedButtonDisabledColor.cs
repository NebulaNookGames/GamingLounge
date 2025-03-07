using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ChangedButtonDisabledColor : MonoBehaviour
{
    public Color newColor = Color.green;
    public Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeButtonDisabledColor);
    }

    public void ChangeButtonDisabledColor()
    {
        ColorBlock colorBlock = button.colors;
        colorBlock.disabledColor = newColor;
        button.colors = colorBlock;
        GetComponentInChildren<TextMeshProUGUI>().enabled = false; 
    }
}