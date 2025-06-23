using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using nn.account;
using System.Text;

public class SwitchSaveSystem<T> : SaveSystem<T> where T : new()
{
    private string _filePath;
    private nn.fs.FileHandle _fileHandle;
    private nn.account.Uid _userId;
    private const string MOUNT_NAME = "saveData";
    private const string SAVE_DATA_PATH = MOUNT_NAME + ":/";

    private static readonly int journalSize = 2048000 - 32768; // 2 MB - system area
    private static readonly int saveDataSize = 4096000; // 4 MB

    public SwitchSaveSystem(string filename) : base(filename)
    {
        _filePath = SAVE_DATA_PATH + filename;
        Account.Initialize();

        UserHandle userHandle = new UserHandle();
        nn.Result result;

#if NN_ACCOUNT_OPENUSER_ENABLE
        Debug.Log("Startup User Account: None - Prompting user to select an account");

        result = Account.ShowUserSelector(ref _userId);
        while (nn.account.Account.ResultCancelledByUser.Includes(result))
        {
            Debug.LogError("User must select an account");
            result = Account.ShowUserSelector(ref _userId);
        }

        result = Account.OpenUser(ref userHandle, _userId);
        if (!result.IsSuccess())
        {
            nn.Nn.Abort($"Failed to open user account: {result.ToString()}");
        }

        result = nn.fs.SaveData.Ensure(_userId);
        if (nn.fs.SaveData.ResultUsableSpaceNotEnoughForSaveData.Includes(result))
        {
            nn.Nn.Abort("Insufficient space for save data.");
        }
#else
        Debug.Log("Startup User Account: Required - Using preselected account");

        if (!Account.TryOpenPreselectedUser(ref userHandle))
        {
            nn.Nn.Abort("TryOpenPreselectedUser failed");
        }

        result = Account.GetUserId(ref _userId, userHandle);
        if (!result.IsSuccess())
        {
            nn.Nn.Abort("Failed to get UserId from preselected user");
        }
#endif

        result = nn.fs.SaveData.Mount(MOUNT_NAME, _userId);
        if (!result.IsSuccess())
        {
            Debug.LogError("Critical Error: File System could not be mounted.");
        }

        Account.CloseUser(userHandle);
    }

    public void Unmount()
    {
        nn.fs.FileSystem.Unmount(MOUNT_NAME);
    }

    public override void Save()
    {
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

        // Ensure file exists or create it
        while (true)
        {
            result = nn.fs.File.Open(ref _fileHandle, _filePath, nn.fs.OpenFileMode.Write);
            if (result.IsSuccess()) break;

            if (nn.fs.FileSystem.ResultPathNotFound.Includes(result))
            {
                result = nn.fs.File.Create(_filePath, dataByteArray.LongLength);
                if (!result.IsSuccess())
                {
                    Debug.LogError($"Failed to create file: {_filePath}, {result.ToString()}");
                    return;
                }
            }
            else
            {
                Debug.LogError($"Failed to open file: {_filePath}, {result.ToString()}");
                return;
            }
        }

        result = nn.fs.File.SetSize(_fileHandle, dataByteArray.LongLength);
        if (nn.fs.FileSystem.ResultUsableSpaceNotEnough.Includes(result))
        {
            Debug.LogError($"Insufficient space to write {dataByteArray.LongLength} bytes to {_filePath}");
            nn.fs.File.Close(_fileHandle);
            return;
        }

        result = nn.fs.File.Write(_fileHandle, 0, dataByteArray, dataByteArray.LongLength, nn.fs.WriteOption.Flush);
        nn.fs.File.Close(_fileHandle);

        if (!result.IsSuccess())
        {
            Debug.LogError($"Failed to write file: {_filePath}, {result.ToString()}");
            return;
        }

        nn.fs.FileSystem.Commit(MOUNT_NAME);

#if UNITY_SWITCH && !UNITY_EDITOR
        UnityEngine.Switch.Notification.LeaveExitRequestHandlingSection();
#endif
    }

    public override void Load()
    {
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
                Debug.LogError($"Failed to open file: {_filePath}, {result.ToString()}");
            }

            LoadingCompleted();
            return;
        }

        long fileSize = 0;
        nn.fs.File.GetSize(ref fileSize, _fileHandle);
        byte[] loadedData = new byte[fileSize];

        result = nn.fs.File.Read(_fileHandle, 0, loadedData, fileSize);
        nn.fs.File.Close(_fileHandle);

        if (!result.IsSuccess())
        {
            Debug.LogError("Critical error: Failed to read save file");
            LoadingCompleted();
            return;
        }

        using (MemoryStream stream = new MemoryStream(loadedData))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            _savedData = (List<T>)formatter.Deserialize(stream);
        }

        LoadingCompleted();
    }
}
