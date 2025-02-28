using UnityEngine;

public class InitializeSceneAfterCutscene : MonoBehaviour
{
    public GameObject player;
    public TutorialInitializer tutorialInitializer;
    public SaveAndLoad saveAndLoad;
    
    private void Awake()
    {
        Invoke(nameof(CheckForCutsceneStopping), 2f);
    }
    
    public void Initialize()
    {
        player.SetActive(true);
        
        StartTutorial();
    }

    void StartTutorial()
    {
        tutorialInitializer.StartInputTutorial();
    }
       
            
    void CheckForCutsceneStopping()
    {
        if (saveAndLoad.saveDataLoaded)
        {
            player.SetActive(true);
            Destroy(gameObject);
        }
    }
}