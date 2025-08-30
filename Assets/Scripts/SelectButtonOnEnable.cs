using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class SelectButtonOnEnable : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject objectToSelect;

    private void OnEnable()
    {
        eventSystem.SetSelectedGameObject(objectToSelect.gameObject);
    }
}
