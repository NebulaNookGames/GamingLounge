using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveInitializer : MonoBehaviour
{
    public static SaveInitializer Instance;
    
    private static string lastSceneName;

    private bool waitingForSaveSystem = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        transform.parent = null;
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        lastSceneName = scene.name;

        // Only try to load if coming from MainMenu
        if (scene.name != "MainMenu" && lastSceneName == "MainMenu")
        {
            StartCoroutine(WaitAndLoadSave());
        }
    }

    private IEnumerator WaitAndLoadSave()
    {
        if (waitingForSaveSystem) yield break;
        waitingForSaveSystem = true;

        // Wait until SaveAndLoad is fully initialized
        while (SaveAndLoad.instance == null || !SaveAndLoad.instance.saveDataLoaded)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // Optional: add another small delay if needed
        yield return new WaitForSeconds(0.2f);

        SaveAndLoad.instance.Load();
        waitingForSaveSystem = false;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}