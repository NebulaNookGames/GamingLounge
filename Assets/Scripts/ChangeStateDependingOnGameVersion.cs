using UnityEngine;

public class ChangeStateDependingOnGameVersion : MonoBehaviour
{
    public bool disableInFullVersion = false;

    public bool disableInDemoVersion = false; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
#if !UNITY_SWITCH
        if (disableInFullVersion && SteamIntegration.instance.isFullVersion || disableInDemoVersion && !SteamIntegration.instance.isFullVersion)
            gameObject.SetActive(false);
#endif
#if UNITY_SWITCH
        gameObject.SetActive(false);
#endif
    }
}
