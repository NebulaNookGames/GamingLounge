using System;
using UnityEngine;

public class ActivateOnTrigger : MonoBehaviour
{
    [SerializeField] private string tagToCheck = "Player";
    [SerializeField] private GameObject[] objectsToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagToCheck))
        {
            foreach (GameObject obj in objectsToActivate)
            {
                obj.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagToCheck))
        {
            foreach (GameObject obj in objectsToActivate)
            {
                obj.SetActive(false);
            }
        }
    }
}
