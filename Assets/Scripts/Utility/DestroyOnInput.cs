using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DestroyOnInput : MonoBehaviour
{

    public InputActionProperty buildMenuInputActionProperty;
    public ActivationTogglerOfOtherGameObject activationTogglerOfOtherGameObject;
    
    
    private void OnEnable()
    {
        buildMenuInputActionProperty.action.performed += Clicked;
    }

    private void OnDisable()
    {
        buildMenuInputActionProperty.action.performed -= Clicked;
    }

    void Clicked(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0) return; 
        
        activationTogglerOfOtherGameObject.OpenContinue();
        Destroy(this.gameObject);
    }
    
}