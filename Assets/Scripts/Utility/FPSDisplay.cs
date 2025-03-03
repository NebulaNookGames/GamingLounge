using System;
using UnityEngine;
using UnityEngine.UI; // Needed if using UI Text
using TMPro;
public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText; // Reference to a UI Text component
    private float deltaTime;
    private float checkInterval = 1f;
    private float timer;

    private void Awake()
    {
        timer = checkInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
      
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f; // Smooth FPS calculation
        float fps = 1.0f / deltaTime;
        if (fps < 60)
            Debug.Log("FPS is lower than 60");
        
        if (timer <= 0)
        {
            timer = checkInterval;
            fpsText.text = Mathf.Ceil(fps).ToString() + " FPS"; // Display FPS
        }
    }
}