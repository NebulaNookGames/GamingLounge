#if UNITY_SWITCH && !UNITY_EDITOR

using UnityEngine;
using System;

public class SwitchSaveHandler : ISaveHandler
{
    private SwitchSaveSystem<SaveDataBinary> switchSaveSystem;

    public SwitchSaveHandler(string filename)
    {
        switchSaveSystem = new SwitchSaveSystem<SaveDataBinary>(filename);
    }

    public void Save(SaveData data)
    {
        try
        {
            SaveDataBinary binaryData = SaveDataBinary.FromSaveData(data);

            var dataSlot = switchSaveSystem.GetData(0);
            if (dataSlot == null)
            {
                // Initialize if null (assuming your SwitchSaveSystem supports this)
                dataSlot = new SaveDataBinary();
                switchSaveSystem.SetData(0, dataSlot);
            }

            dataSlot.CopyFrom(binaryData);
            switchSaveSystem.Save();
            Console.WriteLine("Switch save successful");
        }
        catch (Exception e)
        {
            Console.WriteLine("Switch save failed: " + e.Message);
        }
    }

    public SaveData Load()
    {
        try
        {
            switchSaveSystem.Load();
            SaveDataBinary binaryData = switchSaveSystem.GetData(0);

            if (binaryData == null)
            {
                Console.WriteLine("No save data found.");
                return null;
            }

            SaveData data = binaryData.ToSaveData();
            Console.WriteLine("Switch load successful");
            return data;
        }
        catch (Exception e)
        {
            Console.WriteLine("Switch load failed: " + e.Message);
            return null;
        }
    }

    public void Unmount()
    {
        switchSaveSystem.Unmount();
    }
}
#endif