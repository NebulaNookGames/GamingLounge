#if UNITY_SWITCH
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using nn.account;

public class SwitchSaveSystem<T> : SaveSystem<T> where T : new()
{
    private string _filePath;
    private nn.fs.FileHandle _fileHandle;
    private nn.account.Uid _userId;
    private const string MOUNT_NAME = "CosmicSaveData";
    private const string SAVE_DATA_PATH = MOUNT_NAME + ":/";

    private static readonly int journalSize = 4194304;    // 4 MiB
    private static readonly int saveDataSize = 62914560;  // 60 MiB

    private bool _isMounted = false;  // Track mount state internally

    public SwitchSaveSystem(string filename) : base(filename)
    {
        _filePath = SAVE_DATA_PATH + filename;
        Account.Initialize();

        UserHandle userHandle = new UserHandle();
        nn.Result result;

// #if NN_ACCOUNT_OPENUSER_ENABLE
//         Debug.Log("Startup User Account: None - Prompting user to select an account");
//
//         result = Account.ShowUserSelector(ref _userId);
//         while (nn.account.Account.ResultCancelledByUser.Includes(result))
//         {
//             Debug.LogWarning("User cancelled account selection. Prompting again...");
//             result = Account.ShowUserSelector(ref _userId);
//         }
//
//         if (!result.IsSuccess())
//         {
//             Debug.LogError($"Account.ShowUserSelector failed: {result.ToString()}");
//         }
//
//         result = Account.OpenUser(ref userHandle, _userId);
//         if (!result.IsSuccess())
//         {
//             Debug.LogError($"Account.OpenUser failed: {result.ToString()}");
//         }
//
//         result = nn.fs.SaveData.Ensure(_userId);
//         if (!result.IsSuccess())
//         {
//             if (nn.fs.SaveData.ResultUsableSpaceNotEnoughForSaveData.Includes(result))
//             {
//                 Debug.LogError("Insufficient space for save data.");
//             }
//             else
//             {
//                 Debug.LogError($"SaveData.Ensure failed: {result.ToString()}");
//             }
//         }
// #else
        Debug.Log("Startup User Account: Required - Using preselected account");

        if (!Account.TryOpenPreselectedUser(ref userHandle))
        {
            Debug.LogError("TryOpenPreselectedUser failed");
        }

        result = Account.GetUserId(ref _userId, userHandle);
        if (!result.IsSuccess())
        {
            Debug.LogError($"Failed to get UserId from preselected user: {result.ToString()}");
        }
// #endif

        // Account.CloseUser(userHandle);

        // Initialize _savedData just in case
        if (_savedData == null)
            _savedData = new List<T>();

        if (_savedData.Count == 0)
            _savedData.Add(new T());
    }

    private void Mount()
    {
        if (_isMounted)
            return;

        var result = nn.fs.SaveData.Mount(MOUNT_NAME, _userId);
        if (result.IsSuccess())
        {
            _isMounted = true;
            Debug.Log($"Mounted '{MOUNT_NAME}' successfully.");
        }
        else
        {
            Debug.LogError($"Mount failed: {GetResultDescription(result)}");
            // You may want to throw or handle error here
        }
    }

    public void Unmount()
    {
        if (!_isMounted)
        {
            Debug.Log("Unmount skipped: not mounted.");
            return;
        }

        try
        {
            nn.fs.FileSystem.Unmount(MOUNT_NAME);
            _isMounted = false;
            Debug.Log($"Unmounted '{MOUNT_NAME}' successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Unmount threw exception (likely not mounted): {ex.Message}");
        }
    }

    public override void Save()
    {
        try
        {
            Mount();

            if (_savedData == null)
                _savedData = new List<T>();

            if (_savedData.Count == 0)
                _savedData.Add(new T());

            byte[] dataByteArray;
            using (MemoryStream stream = new MemoryStream(journalSize))
            {
                new BinaryFormatter().Serialize(stream, _savedData);
                dataByteArray = stream.ToArray();
            }

#if UNITY_SWITCH && !UNITY_EDITOR
            UnityEngine.Switch.Notification.EnterExitRequestHandlingSection();
#endif

            nn.Result result;
            _fileHandle = new nn.fs.FileHandle();

            while (true)
            {
                result = nn.fs.File.Open(ref _fileHandle, _filePath, nn.fs.OpenFileMode.Write);
                if (result.IsSuccess()) break;

                if (nn.fs.FileSystem.ResultPathNotFound.Includes(result))
                {
                    result = nn.fs.File.Create(_filePath, dataByteArray.LongLength);
                    if (!result.IsSuccess())
                    {
                        Debug.LogError($"Failed to create file: {_filePath}, {GetResultDescription(result)}");
                        Unmount();
                        return;
                    }
                    else
                    {
                        Debug.Log("Successfully created file.");
                        continue;
                    }
                }
                else
                {
                    Debug.LogError($"Failed to open file: {_filePath}, {GetResultDescription(result)}");
                    Unmount();
                    return;
                }
            }

            result = nn.fs.File.SetSize(_fileHandle, dataByteArray.LongLength);
            if (nn.fs.FileSystem.ResultUsableSpaceNotEnough.Includes(result))
            {
                Debug.LogError($"Insufficient space to write {dataByteArray.LongLength} bytes to {_filePath}");
                nn.fs.File.Close(_fileHandle);
                Unmount();
                return;
            }

            result = nn.fs.File.Write(_fileHandle, 0, dataByteArray, dataByteArray.LongLength, nn.fs.WriteOption.Flush);
            nn.fs.File.Close(_fileHandle);

            if (!result.IsSuccess())
            {
                Debug.LogError($"Failed to write file: {_filePath}, {GetResultDescription(result)}");
                Unmount();
                return;
            }

            result = nn.fs.FileSystem.Commit(MOUNT_NAME);
            if (!result.IsSuccess())
            {
                Debug.LogError($"Failed to commit file system changes for mount {MOUNT_NAME}: {GetResultDescription(result)}");
                Unmount();
                return;
            }

#if UNITY_SWITCH && !UNITY_EDITOR
            UnityEngine.Switch.Notification.LeaveExitRequestHandlingSection();
#endif

            Debug.Log($"Save successful: {_filePath} ({dataByteArray.Length} bytes written)");

            Unmount();
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception during save: " + ex.Message + "\n" + ex.StackTrace);
            Unmount();
        }
    }

    public override void Load()
    {
        try
        {
            Debug.Log("Loading");
            Mount();

            _fileHandle = new nn.fs.FileHandle();

            nn.Result result = nn.fs.File.Open(ref _fileHandle, _filePath, nn.fs.OpenFileMode.Read);
            if (!result.IsSuccess())
            {
                if (nn.fs.FileSystem.ResultPathNotFound.Includes(result))
                {
                    Debug.LogWarning($"File not found: {_filePath}. Save data may not exist yet.");
                }
                else
                {
                    Debug.LogError($"Failed to open file: {_filePath}, {GetResultDescription(result)}");
                }

                LoadingCompleted();
                Unmount();
                return;
            }

            long fileSize = 0;
            nn.fs.File.GetSize(ref fileSize, _fileHandle);

            if (fileSize <= 0)
            {
                Debug.LogWarning($"Save file is empty: {_filePath}");
                nn.fs.File.Close(_fileHandle);
                LoadingCompleted();
                Unmount();
                return;
            }

            byte[] loadedData = new byte[fileSize];

            result = nn.fs.File.Read(_fileHandle, 0, loadedData, fileSize);
            nn.fs.File.Close(_fileHandle);

            if (!result.IsSuccess())
            {
                Debug.LogError("Critical error: Failed to read save file");
                LoadingCompleted();
                Unmount();
                return;
            }

            try
            {
                using (MemoryStream stream = new MemoryStream(loadedData))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    _savedData = (List<T>)formatter.Deserialize(stream);
                }
                Debug.Log($"Load successful: {_filePath} ({fileSize} bytes read)");
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to deserialize save data: " + e.Message);
                _savedData = new List<T>() { new T() };
            }

            LoadingCompleted();
            Unmount();
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception during load: " + ex.Message + "\n" + ex.StackTrace);
            LoadingCompleted();
            Unmount();
        }
    }

    // Data access methods unchanged...
    public T GetData(int index)
    {
        if (_savedData == null || index < 0 || index >= _savedData.Count)
            return default;
        return _savedData[index];
    }

    public void SetData(int index, T data)
    {
        if (_savedData == null)
            _savedData = new List<T>();

        while (_savedData.Count <= index)
            _savedData.Add(new T());

        _savedData[index] = data;
    }

    private string GetResultDescription(nn.Result result)
    {
        return $"Error code: 0x{result.innerValue.ToString("X8")}";
    }
}
#endif
