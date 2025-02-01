using EditorAttributes; 
using UnityEngine;

public class SteamIntegration : MonoBehaviour
{
    public static SteamIntegration instance;

    private void Awake()
    {
        instance = this; 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        try
        {
            Steamworks.SteamClient.Init(3426730);
            PrintYourName();
            DontDestroyOnLoad(this);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void PrintYourName()
    {
        Debug.Log(Steamworks.SteamClient.Name);
    }

    private void Update()
    {
        Steamworks.SteamClient.RunCallbacks();
    }

    void OnApplicationQuit()
    {
        Steamworks.SteamClient.Shutdown();
    }

    [Button]
    public void IsThisAchievementUnlocked(string id)
    {
        var ach = new Steamworks.Data.Achievement();
        Debug.Log("Achievement " + id + " status: " + ach.State);
    }

    [Button]
    public void UnlockAchievement(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);
        ach.Trigger();
        Debug.Log("Achievement " + id + " unlocked");

    }

    [Button]
    public void ClearAchievementStatus(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);
        ach.Clear();
        Debug.Log("Achievement " + id + " cleared");
    }
}