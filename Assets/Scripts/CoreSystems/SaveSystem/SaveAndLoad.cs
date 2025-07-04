using UnityEngine;
using System;
using System.IO;
using System.Collections;

public class SaveAndLoad : MonoBehaviour
{
    public static SaveAndLoad instance;

    [Tooltip("The data handlers which should receive and send data as a SaveData object.")]
    [SerializeField] private DataHandler[] dataHandlers;

    public Action onDataLoaded;

    [Header("Auto-Save Settings")]
    [SerializeField] private bool autoSaveEnabled = true;
    [SerializeField] private float autoSaveInterval = 300f;

    public SaveData loadedSaveData;
    public bool saveDataLoaded = false;
    public GameObject autoSaveCanvas;
    private bool savedSecondTimeThisTime = false;

    private ISaveHandler saveHandler;

#if UNITY_SWITCH && !UNITY_EDITOR
    private SwitchSaveHandler switchSaveHandler;
#endif

    private string mainSavePath => Path.Combine(Application.persistentDataPath, "saveFile.json");
    private string backupSavePath => Path.Combine(Application.persistentDataPath, "saveDataTwo.json");

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
#if UNITY_SWITCH && !UNITY_EDITOR
        StartCoroutine(InitializeSwitchSaveHandler());
#else
        saveHandler = new JsonSaveHandler(mainSavePath, backupSavePath);
        // Load(); // Load is now handled by SaveInitializer
#endif

        if (autoSaveEnabled)
        {
            StartAutoSave();
        }
    }

#if UNITY_SWITCH && !UNITY_EDITOR
    private IEnumerator InitializeSwitchSaveHandler()
    {
        yield return new WaitForSeconds(0.2f); // Wait for Switch file systems to initialize

        try
        {
            switchSaveHandler = new SwitchSaveHandler("SwitchSave.bin");
            saveHandler = switchSaveHandler;

            // Load(); // Load is now handled by SaveInitializer
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to initialize Switch save system: " + e.Message);
        }
    }
#endif

    public void Save()
    {
        try
        {
            SaveData saveData = new SaveData();
            foreach (DataHandler dataHandler in dataHandlers)
            {
                dataHandler.SendData(saveData);
            }

            saveHandler.Save(saveData);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Failed saving file: " + ex.Message);
        }
    }

    public void Load()
    {
        try
        {
            SaveData saveData = saveHandler.Load();
            if (saveData == null)
            {
                Debug.LogWarning("Save file not found or is empty.");
                return;
            }

            loadedSaveData = saveData;
            foreach (DataHandler dataHandler in dataHandlers)
            {
                dataHandler.ReceiveData(loadedSaveData);
            }

            onDataLoaded?.Invoke();
            saveDataLoaded = true;
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Failed loading file: " + ex.Message);
        }
    }

    private void OnApplicationQuit()
    {
#if UNITY_SWITCH && !UNITY_EDITOR
        switchSaveHandler?.Unmount();
#endif
    }

    private void DisableAutoSaveCanvas()
    {
        if (autoSaveCanvas != null)
            autoSaveCanvas.SetActive(false);
    }

    public void StartAutoSave() => InvokeRepeating(nameof(AutoSave), autoSaveInterval, autoSaveInterval);
    public void StopAutoSave() => CancelInvoke(nameof(AutoSave));

    private void AutoSave()
    {
        Debug.Log("Auto-Saving...");
        if (autoSaveCanvas != null)
        {
            autoSaveCanvas.SetActive(true);
            Invoke(nameof(DisableAutoSaveCanvas), 7f);
        }
        Save();
    }
}
