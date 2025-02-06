using System;
using UnityEngine;

public class TutorialInitializer : MonoBehaviour
{
    [SerializeField] private SaveAndLoad saveAndLoad;
    [SerializeField] private TutorialPlacement tutorialPlacement;
    [SerializeField] private GameObject moveTutorial; 
    private void Awake()
    {
        Invoke(nameof(CheckForTutorialActivation), 2f);
    }

    private void CheckForTutorialActivation()
    {
        if (!saveAndLoad.saveDataLoaded)
        {
            tutorialPlacement.enabled = true;
           Invoke(nameof(StartInputTutorial), 4f);
        }
        else
        {
            InputManager.instance.placementInputUnlocked = true; 
            Destroy(this.gameObject);
        }
    }

    void StartInputTutorial()
    {
        moveTutorial.SetActive(true);
    }

    public void StopInvoke()
    {
        if(IsInvoking(nameof(CheckForTutorialActivation)))
            CancelInvoke(nameof(CheckForTutorialActivation));
        
        if(IsInvoking(nameof(StartInputTutorial)))
            CancelInvoke(nameof(StartInputTutorial));
    }
}