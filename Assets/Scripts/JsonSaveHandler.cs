using System.IO;
using UnityEngine;

public class JsonSaveHandler : ISaveHandler
{
    private string mainPath;
    private string backupPath;
    private bool backupWritten = false;

    public JsonSaveHandler(string mainPath, string backupPath)
    {
        this.mainPath = mainPath;
        this.backupPath = backupPath;
    }

    public void Save(SaveData data)
    {
        string jsonString = JsonUtility.ToJson(data, true);
        File.WriteAllText(mainPath + ".tmp", jsonString);

        if (File.Exists(mainPath))
            File.Delete(mainPath);
        File.Move(mainPath + ".tmp", mainPath);

        if (!backupWritten)
        {
            File.WriteAllText(backupPath + ".tmp", jsonString);
            if (File.Exists(backupPath))
                File.Delete(backupPath);
            File.Move(backupPath + ".tmp", backupPath);
            backupWritten = true;
        }
    }

    public SaveData Load()
    {
        if (!File.Exists(mainPath))
            return null;

        string jsonString = File.ReadAllText(mainPath);
        return JsonUtility.FromJson<SaveData>(jsonString);
    }
}