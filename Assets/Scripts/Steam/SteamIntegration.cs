using EditorAttributes;
using UnityEngine;

public class SteamIntegration : MonoBehaviour
{
    #region Singleton

    public static SteamIntegration instance;

    public bool isFullVersion = true; 
    
    private void Awake()
    {
        if(instance == null) 
            instance = this;
        else
            Destroy(gameObject);
    }

    #endregion

    #region Unity Lifecycle Methods

    void Start()
    {
        try
        {
            
#if !UNITY_SWITCH
            InitializeSteam();
            SetLocaleBasedOnSteamLanguage();
#endif
            DontDestroyOnLoad(this);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error initializing Steam: {e.Message}");
        }
    }

    void Update()
    {
#if !UNITY_SWITCH
        Steamworks.SteamClient.RunCallbacks();
#endif
    }

    void OnApplicationQuit()
    {
#if !UNITY_SWITCH
        Steamworks.SteamClient.Shutdown();
#endif
    }

    #endregion

    #region Steam Initialization & Locale

    private void InitializeSteam()
    {
        if(isFullVersion) 
            Steamworks.SteamClient.Init(3426730);  // App ID for your game on Steam
        else
            Steamworks.SteamClient.Init(3526570);
        
        PrintYourName();
    }

    private void SetLocaleBasedOnSteamLanguage()
    {
        if (LocaleSelector.Instance)
        {
            switch (Steamworks.SteamApps.GameLanguage)
            {
                case "german":
                    LocaleSelector.Instance.ChangeLocale(1);
                    break;
                case "chinese":
                    LocaleSelector.Instance.ChangeLocale(2);
                    break;
                case "japanese":
                    LocaleSelector.Instance.ChangeLocale(3);
                    break;
                case "portuguese":
                    LocaleSelector.Instance.ChangeLocale(4);
                    break;
                case "russian":
                    LocaleSelector.Instance.ChangeLocale(5);
                    break;
                case "spanish":
                    LocaleSelector.Instance.ChangeLocale(6);
                    break;
                default:
                    LocaleSelector.Instance.ChangeLocale(0);
                    break;
            }
        }
    }

    #endregion

    #region Debugging & Logging

    private void PrintYourName()
    {
        Debug.Log(Steamworks.SteamClient.Name);
    }

    #endregion

    #region Achievement Methods

    [Button]
    public void IsThisAchievementUnlocked(string id)
    {
        if (!isFullVersion) return; 
        
        var ach = new Steamworks.Data.Achievement();
        Debug.Log("Achievement " + id + " status: " + ach.State);
    }

    [Button]
    public void UnlockAchievement(string id)
    {
        if (!isFullVersion) return; 
        
        var ach = new Steamworks.Data.Achievement(id);
        ach.Trigger();
        Debug.Log("Achievement " + id + " unlocked");
    }

    [Button]
    public void ClearAchievementStatus(string id)
    {
        if (!isFullVersion) return; 
        
        var ach = new Steamworks.Data.Achievement(id);
        ach.Clear();
        Debug.Log("Achievement " + id + " cleared");
    }

    [Button] // This adds a button in the Unity Editor if you use Odin Inspector
    public void ClearAllAchievements()
    {
        if (!isFullVersion) return; 
        
        string[] achievementIDs = {
            "TIER2STRUCTUREUNLOCK", "TIER3STRUCTUREUNLOCK", "CRYSTALUNLOCK", 
            "LIGHTSABERUNLOCK", "FLUXCAPACITORUNLOCK", "TRAFFICCONEUNLOCK", 
            "COUCHUNLOCK", "TROPHYUNLOCK", "ARCADEMACHINEUNLOCK", 
            "BIKEMACHINEUNLOCK", "RACINGMACHINEUNLOCK", "PLANTUNLOCK", 
            "CHAIRUNLOCK", "SPRINGNATUREUNLOCK", "FALLNATUREUNLOCK", 
            "CANDYLANDNATUREUNLOCK", "ROCKUNLOCK", "UPGRADEPCUNLOCK", 
            "THIRTYVISITORS", "FIFTYVISITORS", "ONEHUNDREDVISITORS"
        };

        foreach (string id in achievementIDs)
        {
            var ach = new Steamworks.Data.Achievement(id);
            ach.Clear();
            Debug.Log($"Cleared achievement: {id}");
        }
    }

    #endregion
}
