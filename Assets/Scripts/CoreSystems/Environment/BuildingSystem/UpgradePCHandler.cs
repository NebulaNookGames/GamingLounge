using System;
using UnityEngine;

public class UpgradePCHandler : MonoBehaviour
{
    private bool isActive = false;
    public GameObject inDistanceProof;
    GameObject UpgradePCCanvas;

    private void OnEnable()
    {
        UpgradePCCanvas = GameObject.FindWithTag("UpgradeCanvas");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && inDistanceProof.activeSelf && !GameObject.FindWithTag("MenuUI").GetComponent<PauseHandler>().IsPaused)
            ChangeActiveState();
    }

    void ChangeActiveState()
    {
        if (isActive)
        {
            GameObject.FindWithTag("MenuUI").GetComponent<PauseHandler>().pausedByOtherSystem = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            UpgradePCCanvas.GetComponent<UpgradeCanvasHandler>().SetUpgradePanelState(false);
            Time.timeScale = 1;
        }
        else
        {
            GameObject.FindWithTag("MenuUI").GetComponent<PauseHandler>().pausedByOtherSystem = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; 
            UpgradePCCanvas.GetComponent<UpgradeCanvasHandler>().SetUpgradePanelState(true);
            Time.timeScale = 0;
        }
        isActive = !isActive;
    }
}
