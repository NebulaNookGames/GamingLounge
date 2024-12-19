using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveInitializer : MonoBehaviour
{
    public static SaveInitializer Instance;
    
    private static string lastSceneName; // Stores the name of the last scene

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(gameObject);
        }
        // Ensure this object persists across scenes
        DontDestroyOnLoad(this.gameObject);

        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Do something if the last scene matches your condition
        if (lastSceneName == "MainMenu")
        {
            Invoke("LoadSaveFile", 1f);
        }

        // Update the lastSceneName to the current scene's name
        lastSceneName = scene.name;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event when this object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void LoadSaveFile()
    {
        SaveAndLoad.instance.Load();
    }
}
