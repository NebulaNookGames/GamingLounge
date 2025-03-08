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

            if (LocaleSelector.Instance)
            {
                switch (Steamworks.SteamApps.GameLanguage)
                {
                    case "german":
                        LocaleSelector.Instance.ChangeLocale(1);
                        break;

                    default:
                        LocaleSelector.Instance.ChangeLocale(0);
                        break;
                }
            }

            DontDestroyOnLoad(this);
        }
        catch (System.Exception e)
        {
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
    
    [Button] // This adds a button in the Unity Editor if you use Odin Inspector
    public void ClearAllAchievements()
    {
        string[] achievementIDs = {
            "TIER2STRUCTUREUNLOCK",
            "TIER3STRUCTUREUNLOCK",
            "CRYSTALUNLOCK",
            "LIGHTSABERUNLOCK",
            "FLUXCAPACITORUNLOCK",
            "TRAFFICCONEUNLOCK",
            "COUCHUNLOCK",
            "TROPHYUNLOCK",
            "ARCADEMACHINEUNLOCK",
            "BIKEMACHINEUNLOCK",
            "RACINGMACHINEUNLOCK",
            "PLANTUNLOCK",
            "CHAIRUNLOCK",
            "SPRINGNATUREUNLOCK",
            "FALLNATUREUNLOCK",
            "CANDYLANDNATUREUNLOCK",
            "ROCKUNLOCK",
            "UPGRADEPCUNLOCK",
            "THIRTYVISITORS",
            "FIFTYVISITORS",
            "ONEHUNDREDVISITORS"
        };
        
        foreach (string id in achievementIDs)
        {
            var ach = new Steamworks.Data.Achievement(id);
            ach.Clear();
            Debug.Log($"Cleared achievement: {id}");
        }
    }

}