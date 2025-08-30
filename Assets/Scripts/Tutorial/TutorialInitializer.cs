using System;
using UnityEngine;

public class TutorialInitializer : MonoBehaviour
{
    [SerializeField] private SaveAndLoad saveAndLoad;
    [SerializeField] private TutorialPlacement tutorialPlacement;
    [SerializeField] private GameObject moveTutorial;
    [SerializeField] private GameObject tutorialBackground;
    public GameObject placementHelpIcon; 
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
            placementHelpIcon.SetActive(true);
            Destroy(gameObject);
        }
    }

    public void StartInputTutorial()
    {
        Invoke(nameof(ActivateTutorialUI), 2f);
        playerCam.SetActive(true);
    }

    void ActivateTutorialUI()
    {
        moveTutorial.SetActive(true);
        tutorialBackground.SetActive(true);
    }


    public void StopInvoke()
    {
        if(IsInvoking(nameof(CheckForTutorialActivation)))
            CancelInvoke(nameof(CheckForTutorialActivation));
        
        if(IsInvoking(nameof(StartInputTutorial)))
            CancelInvoke(nameof(StartInputTutorial));
    }
}