using UnityEngine;
using System;
using System.IO;

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

    private string mainSavePath => Path.Combine(Application.persistentDataPath, "saveFile.json");
    private string backupSavePath => Path.Combine(Application.persistentDataPath, "saveDataTwo.json");

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }

        if (autoSaveEnabled)
        {
            StartAutoSave();
        }
        
    }

    public void StartAutoSave()
    {
        InvokeRepeating(nameof(AutoSave), autoSaveInterval, autoSaveInterval);
    }

    public void StopAutoSave()
    {
        CancelInvoke(nameof(AutoSave));
    }

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

    public void Save()
    {
        try
        {
            SaveData saveData = new SaveData();

            foreach (DataHandler dataHandler in dataHandlers)
            {
                dataHandler.SendData(saveData);
            }

            string jsonString = JsonUtility.ToJson(saveData, true);

            // --- SAFE SAVE (write to temp file first) ---
            string tempPath = mainSavePath + ".tmp";
            File.WriteAllText(tempPath, jsonString);

            if (File.Exists(mainSavePath))
                File.Delete(mainSavePath);

            File.Move(tempPath, mainSavePath);

            // --- BACKUP SAVE ---
            if (!savedSecondTimeThisTime)
            {
                savedSecondTimeThisTime = true;

                string tempBackupPath = backupSavePath + ".tmp";
                File.WriteAllText(tempBackupPath, jsonString);

                if (File.Exists(backupSavePath))
                    File.Delete(backupSavePath);

                File.Move(tempBackupPath, backupSavePath);
            }
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
            if (!File.Exists(mainSavePath))
            {
                Debug.LogWarning("Save file not found!");
                return;
            }

            string jsonString = File.ReadAllText(mainSavePath);

            loadedSaveData = JsonUtility.FromJson<SaveData>(jsonString);

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

    private void DisableAutoSaveCanvas()
    {
        if (autoSaveCanvas != null)
            autoSaveCanvas.SetActive(false);
    }
}
