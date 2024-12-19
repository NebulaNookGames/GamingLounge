using UnityEngine;
using System; 
using System.IO;

/// <summary>
/// Saves and Loads information from and to a JSON file. 
/// </summary>
public class SaveAndLoad : MonoBehaviour
{
    // Singleton of this class.
    public static SaveAndLoad instance; 
    
    [Tooltip("The data handlers which should receive and send data as a saveData object.")]
    [SerializeField] DataHandler[] dataHandlers;

    public Action onDataLoaded;
    
    #region Methods
    /// <summary>
    /// Creates a singleton of this instance if a singleton has not been set.
    /// </summary>
    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }
    
    // /// <summary>
    // /// Calls the LoadAtGameStart method.
    // /// </summary>
    // void Start()
    // {
    //      LoadAtGameStart();
    // }

    /// <summary>
    /// Loads the json file and if it fails, debug to the console.
    /// </summary>
    void LoadAtGameStart()
    {
        try
        {
            Load();
        }
        catch
        {
            Debug.Log("No Json saveFile found.");
        }
    }

    /// <summary>
    /// Save the variables from a new savedata object to a json file. 
    /// </summary>
    public void Save()
    {
        File.Delete(Application.persistentDataPath + "/saveFile.json");
        SaveData saveData = new SaveData();

        foreach (DataHandler dataHandler in dataHandlers) // Recieve data from all data handlers.
        {
            dataHandler.SendData(saveData);
        }

        string jsonString = JsonUtility.ToJson(saveData, true); // Create a new string for saving as json file data.

        File.WriteAllText(Application.persistentDataPath + "/saveFile.json", jsonString); // Save the string in a json file.
      
        Debug.Log("Saved successfully");
    }

    /// <summary>
    /// Loads the variables from a json file and stores them in a new savedata object.
    /// </summary>
    public void Load()
    {
        string jsonString = File.ReadAllText(Application.persistentDataPath + "/saveFile.json"); // Save the data into a string.

        SaveData saveData = JsonUtility.FromJson<SaveData>(jsonString); // Save the string information to a save data object.

        foreach (DataHandler dataHandler in dataHandlers) // Distribute the save data information to each data handler.
        {
            dataHandler.ReceiveData(saveData);
        }
        
        Debug.Log("Loaded successfully");
        onDataLoaded?.Invoke();
    }
    #endregion 

}