using UnityEngine;
using UnityEngine.UI; // Needed if using UI Text
using TMPro;
public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText; // Reference to a UI Text component
    private float deltaTime;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f; // Smooth FPS calculation
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString() + " FPS"; // Display FPS
        if (fps < 60)
        {
            Debug.Log("FPS is lower than 60");
        }
    }
}