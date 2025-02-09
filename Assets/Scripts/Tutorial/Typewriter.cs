using TMPro;
using UnityEngine;

public class Typewriter : MonoBehaviour
{
    [Header("Typewriter Settings")]
    public float startDelay = 1.0f;                 // Initial delay before typing starts
    public float speed = 0.2f;                      // Delay between characters
    string textToWrite = "";                        // Full text to display
    public TextMeshProUGUI text;                    // Reference to the UI Text

    [Header("Sound Settings")]
    public AudioClip typeSound;                     // Typing sound effect
    public float soundPitchVariation = 0.2f;        // Increased pitch variation
    public float soundVolume = 1f;                // Lowered volume for less annoyance
    public float soundPlayChance = 0.7f;            // 70% chance to play the sound
    public AudioSource audioSource;                // AudioSource to play sounds

    private float timer;                            // Countdown timer for character delay
    private float delayTimer;                       // Countdown timer for initial delay
    private int textIndex = 0;                      // Current character index
    private bool isTyping = false;                  // Flag to start typing after delay

    private void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
        textToWrite = text.text; 
        ResetTypewriter();                          // Ensure it's clean when enabled
    }

    private void Update()
    {
        // Handle initial delay before typing starts
        if (!isTyping)
        {
            delayTimer -= Time.deltaTime;
            if (delayTimer <= 0)
            {
                isTyping = true;                    // Start typing after delay
            }
            return;
        }

        // Typewriter effect logic
        if (textIndex < textToWrite.Length)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = speed;
                text.text += textToWrite[textIndex]; // Add next character

                if (Random.value < soundPlayChance) // Random chance to play sound
                {
                    PlayTypingSound();              // Play typing sound effect
                }

                textIndex++;
            }
        }
        else
        {
            enabled = false;                         // Disable script when done
        }
    }

    private void PlayTypingSound()
    {
        if (typeSound != null && audioSource != null)
        {
            audioSource.pitch = 1f + Random.Range(-soundPitchVariation, soundPitchVariation);
            audioSource.volume = soundVolume;       // Adjust volume
            audioSource.PlayOneShot(typeSound);
        }
    }

    public void ResetTypewriter()
    {
        timer = speed;
        delayTimer = startDelay;                     // Reset initial delay timer
        textIndex = 0;
        text.text = "";
        isTyping = false;                           // Prevent typing until delay finishes
        enabled = true;                             // Reactivate script if needed
    }

    public void SkipToEnd()
    {
        text.text = textToWrite;
        enabled = false;
    }
}
