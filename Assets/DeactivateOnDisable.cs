using System;
using UnityEngine;

public class DeactivateOnDisable : MonoBehaviour
{
    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
