using System;
using UnityEngine;

public class DeactivateAllInArrayOnDisable : MonoBehaviour
{
    public GameObject[] objectsToDeactivate;

    private void OnDisable()
    {
        foreach (GameObject obj in objectsToDeactivate)
        {
            obj.SetActive(false);
        }
    }
}
