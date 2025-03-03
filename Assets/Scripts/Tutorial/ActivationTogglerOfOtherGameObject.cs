using System;
using NUnit.Framework.Constraints;
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
    public AudioSource audioS;
    public AudioClip audioC;
    public bool canContinue = true;
    
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
    
    public void Continue(InputAction.CallbackContext context)
    {
        if (!canContinue) return; 
        
        if (Time.timeScale == 0)
            return;
        
        audioS.PlayOneShot(audioC);
        DisableThisAndActivateOther();
    }

    public void OpenContinue()
    {
        if (Time.timeScale == 0)
            return;
        
        audioS.PlayOneShot(audioC);
        DisableThisAndActivateOther();
    }
    

    void DisableThisAndActivateOther()
    {
        if (isLastTutorial)
        {
            if(background)
                background.SetActive(false);
            Destroy(playerCam);
        }
        
        if(gameObjectToActivate != null)
            gameObjectToActivate.SetActive(true);
        
        gameObject.SetActive(false);
    }
}
