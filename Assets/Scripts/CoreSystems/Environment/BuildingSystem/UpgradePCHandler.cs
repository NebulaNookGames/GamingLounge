using System;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem; 

public class UpgradePCHandler : MonoBehaviour
{
    private bool isActive = false;
    public GameObject inDistanceProof;
    GameObject UpgradePCCanvas;

    public InputActionProperty worldObjectUsageAction; 
    
    private void OnEnable()
    {
        UpgradePCCanvas = GameObject.FindWithTag("UpgradeCanvas");
        worldObjectUsageAction.action.performed += UseObject;
    }

    private void OnDisable()
    {
        worldObjectUsageAction.action.performed -= UseObject;
    }

    void UseObject(InputAction.CallbackContext context)
    {
        if(inDistanceProof.activeSelf && !GameObject.FindWithTag("MenuUI").GetComponent<PauseHandler>().IsPaused)
            ChangeActiveState();
    }

    void ChangeActiveState()
    {
        if (isActive)
        {
            GameObject.FindWithTag("MenuUI").GetComponent<PauseHandler>().pausedByOtherSystem = false;
            Cursor.lockState = CursorLockMode.Locked;
            UpgradePCCanvas.GetComponent<UpgradeCanvasHandler>().SetUpgradePanelState(false);
            Time.timeScale = 1;
        }
        else
        {
            GameObject.FindWithTag("MenuUI").GetComponent<PauseHandler>().pausedByOtherSystem = true;
            Cursor.lockState = CursorLockMode.None; 
            UpgradePCCanvas.GetComponent<UpgradeCanvasHandler>().SetUpgradePanelState(true);
            Time.timeScale = 0;
        }
        isActive = !isActive;
    }
}
