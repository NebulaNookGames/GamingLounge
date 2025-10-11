using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
#if UNITY_SWITCH
        Application.targetFrameRate = 30; 
#endif 
#if !UNITY_SWITCH
        Application.targetFrameRate = 60;
#endif
        QualitySettings.vSyncCount = 0;
    }
}