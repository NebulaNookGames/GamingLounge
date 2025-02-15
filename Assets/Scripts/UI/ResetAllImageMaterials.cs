using System;
using UnityEngine;
using UnityEngine.UI;
public class ResetAllImageMaterials : MonoBehaviour
{
    public Image[] images;
    public Material material;

    private void OnDisable()
    {
        foreach (Image image in images)
        {
            image.material = material;
        }
    }
}
