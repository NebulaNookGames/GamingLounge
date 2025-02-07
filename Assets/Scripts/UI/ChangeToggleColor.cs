using System;
using UnityEngine;
using UnityEngine.UI;

public class ChangeToggleColor : MonoBehaviour
{
    public Color toggleColor;
    public Color baseColor;
    public Image toggleBackgroundImage;

    private void Awake()
    {
        baseColor = toggleBackgroundImage.color;
    }

    public void ChangeColor(bool useToggleColor)
    {
        if(useToggleColor)
            toggleBackgroundImage.color = toggleColor;
        else
            toggleBackgroundImage.color = baseColor;
    }
}
