using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Application.targetFrameRate = 60; // Set to 30, 60, or any preferred FPS
            QualitySettings.vSyncCount = 0; // Disable V-Sync to ensure FPS cap works properly
    }
}