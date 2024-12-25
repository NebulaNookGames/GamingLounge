using UnityEngine;
using System;
using System.IO;

public class SaveAndLoad : MonoBehaviour
{
    public static SaveAndLoad instance;

    [Tooltip("The data handlers which should receive and send data as a saveData object.")]
    [SerializeField] DataHandler[] dataHandlers;

    public Action onDataLoaded;

    [Header("Auto-Save Settings")]
    [Tooltip("Enable or disable auto-save.")]
    [SerializeField] private bool autoSaveEnabled = true;

    [Tooltip("Interval in seconds between auto-saves.")]
    [SerializeField] private float autoSaveInterval = 300f;

    private SaveData loadedSaveData; 
    
    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }

        if (autoSaveEnabled)
        {
            StartAutoSave();
        }
    }

    /// <summary>
    /// Starts the auto-save routine.
    /// </summary>
    public void StartAutoSave()
    {
        InvokeRepeating(nameof(AutoSave), autoSaveInterval, autoSaveInterval);
    }

    /// <summary>
    /// Stops the auto-save routine.
    /// </summary>
    public void StopAutoSave()
    {
        CancelInvoke(nameof(AutoSave));
    }

    /// <summary>
    /// Auto-save functionality triggered by InvokeRepeating.
    /// </summary>
    private void AutoSave()
    {
        Debug.Log("Auto-Saving...");
        Save();
    }

    /// <summary>
    /// Save the variables from a new savedata object to a JSON file.
    /// </summary>
    public void Save()
    {
        try
        {
            File.Delete(Application.persistentDataPath + "/saveFile.json");
            SaveData saveData = new SaveData();

            foreach (DataHandler dataHandler in dataHandlers) // Recieve data from all data handlers.
            {
                dataHandler.SendData(saveData);
            }

            string jsonString = JsonUtility.ToJson(saveData, true); // Create a new string for saving as JSON file data.

            File.WriteAllText(Application.persistentDataPath + "/saveFile.json",
                jsonString); // Save the string in a JSON file.
        }
        catch
        {
            Debug.Log("Failed saving File.");
        }
    }

    /// <summary>
    /// Loads the variables from a JSON file and stores them in a new savedata object.
    /// </summary>
    public void Load()
    {
            string jsonString =
                File.ReadAllText(Application.persistentDataPath + "/saveFile.json"); // Save the data into a string.

            loadedSaveData =
                JsonUtility.FromJson<SaveData>(jsonString); // Save the string information to a save data object.

            foreach (DataHandler dataHandler in
                     dataHandlers) // Distribute the save data information to each data handler.
            {
                dataHandler.ReceiveData(loadedSaveData);
            }

            onDataLoaded?.Invoke();
        
    }
}