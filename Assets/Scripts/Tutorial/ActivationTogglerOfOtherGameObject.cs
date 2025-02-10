using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActivationTogglerOfOtherGameObject : MonoBehaviour
{
    public GameObject gameObjectToActivate;
    public GameObject background; 
    
    public InputActionProperty continueAction;
    public bool isLastTutorial;
    public GameObject playerCam; 
    private void OnEnable()
    {
        continueAction.action.performed += Continue;
        if(background)
            background.SetActive(true);
    }
    
    private void OnDisable()
    {
        continueAction.action.performed -= Continue;

        if(IsInvoking(nameof(DisableThisAndActivateOther)))
            CancelInvoke(nameof(DisableThisAndActivateOther));
    }
    
    void Continue(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0)
            return;
        
        Debug.Log("Continuing");
        DisableThisAndActivateOther();
    }
    

    void DisableThisAndActivateOther()
    {
        if(isLastTutorial)
            Destroy(playerCam);
            
        if(background)
            background.SetActive(false);
        
        if(gameObjectToActivate != null)
            gameObjectToActivate.SetActive(true);
        
        gameObject.SetActive(false);
    }
}
