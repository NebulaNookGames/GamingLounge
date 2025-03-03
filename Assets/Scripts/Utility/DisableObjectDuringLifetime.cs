using System;
using UnityEngine;

public class DisableObjectDuringLifetime : MonoBehaviour
{
    public GameObject objectToDisable;

    private void OnEnable()
    {
        objectToDisable.SetActive(false);
    }

    private void OnDisable()
    {
        objectToDisable.SetActive(true);
    }
}
