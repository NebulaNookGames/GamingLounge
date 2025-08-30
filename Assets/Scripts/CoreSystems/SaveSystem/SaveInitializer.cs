using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveInitializer : MonoBehaviour
{
    public static SaveInitializer Instance;

    private static string lastSceneName;
    private bool waitingForSaveSystem = false;
    private bool hasLoadedSaveData = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Also load when transitioning away from MainMenu, if needed
        if (scene.name != "MainMenu" && lastSceneName == "MainMenu")
        {
            StartCoroutine(WaitAndLoadSave());
        }

        lastSceneName = scene.name;
    }

    private IEnumerator WaitAndLoadSave()
    {
        yield return new WaitForSeconds(1f);
        
        waitingForSaveSystem = true;
        
        // Call load once ready
        SaveAndLoad.instance.Load();

        hasLoadedSaveData = true;
        waitingForSaveSystem = false;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}