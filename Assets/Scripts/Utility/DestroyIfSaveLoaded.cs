using System;
using UnityEngine;

public class DestroyIfSaveLoaded : MonoBehaviour
{
    private void Awake()
    {
        if (SaveAndLoad.instance.saveDataLoaded)
        {
            Destroy(gameObject);
        }
    }
}
