using System;
using UnityEngine;
using UnityEngine.EventSystems; 
using UnityEngine.UI;

public class SetDynamicSelectedUIElement : MonoBehaviour
{
    public EventSystem eventSystem;

    public bool initializeButtonOnSelection = false; 
    private void OnEnable()
    {
        Change();
    }
    
    public void Change()
    {
        if (eventSystem == null || GameInput.Instance != null && GameInput.Instance.activeGameDevice == GameInput.GameDevice.KeyboardMouse) return;
        
        Button buttonToUse = GetComponentInChildren<Button>();
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            if (button.interactable)
            {
                buttonToUse = button;
                break;
            }
        }
        
        if (buttonToUse == null) return; 
        
        eventSystem.SetSelectedGameObject(buttonToUse.gameObject);

        if (initializeButtonOnSelection)
            buttonToUse.onClick.Invoke();
    }
}
