using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class UpgradeCanvasHandler : MonoBehaviour
{
    public GameObject UpgradePanel;
    public List<GameObject> panelsToDisableOnEnable;
    public List<GameObject> panelsToEnableOnEnable; 
    
    public List<GameObject> panelsToDisableOnDisable;
    
    public void SetUpgradePanelState(bool activeState)
    {
        UpgradePanel.SetActive(activeState);

        if (activeState)
        {
            foreach (GameObject panel in panelsToDisableOnEnable)
            {
                panel.SetActive(false);
            }
            foreach (GameObject panel in panelsToEnableOnEnable)
            {
                panel.SetActive(true);
            }
            
            GameInput.Instance.SetMouseVisibility(true);
        }
        
        if (!activeState)
        {
            foreach (GameObject panel in panelsToDisableOnDisable)
            {
                panel.SetActive(false);
            }
            GameInput.Instance.SetMouseVisibility(false);
        }
    }
}
