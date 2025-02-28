using System;
using UnityEngine;

public class TutorialInitializer : MonoBehaviour
{
    [SerializeField] private SaveAndLoad saveAndLoad;
    [SerializeField] private TutorialPlacement tutorialPlacement;
    [SerializeField] private GameObject moveTutorial;
    [SerializeField] private GameObject tutorialBackground;
    
    public GameObject playerCam; 
    private void Awake()
    {
        Invoke(nameof(CheckForTutorialActivation), 5f);
    }

    private void CheckForTutorialActivation()
    {
        if (!saveAndLoad.saveDataLoaded)
        {
            tutorialPlacement.enabled = true;
        }
        else
        {
            InputManager.instance.placementInputUnlocked = true; 
            Destroy(gameObject);
        }
    }

    public void StartInputTutorial()
    {
        moveTutorial.SetActive(true);
        tutorialBackground.SetActive(true);
        playerCam.SetActive(true);
    }

    public void StopInvoke()
    {
        if(IsInvoking(nameof(CheckForTutorialActivation)))
            CancelInvoke(nameof(CheckForTutorialActivation));
        
        if(IsInvoking(nameof(StartInputTutorial)))
            CancelInvoke(nameof(StartInputTutorial));
    }
}