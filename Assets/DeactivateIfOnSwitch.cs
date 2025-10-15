using System;
using UnityEngine;

public class DeactivateIfOnSwitch : MonoBehaviour
{
    private void Awake()
    {
        #if UNITY_SWITCH && !UNITY_EDITOR
        gameObject.SetActive(false);
        #endif
    }
}