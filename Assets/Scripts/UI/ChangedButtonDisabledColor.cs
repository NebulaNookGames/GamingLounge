using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ChangedButtonDisabledColor : MonoBehaviour
{
    public Color newColor = Color.green;
    public Color newSelectedColor = Color.blue;
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
        colorBlock.normalColor = newColor;
        colorBlock.selectedColor = newSelectedColor;
        button.colors = colorBlock;
        GetComponentInChildren<TextMeshProUGUI>().enabled = false; 
    }
}