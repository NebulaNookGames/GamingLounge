using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioMixer audioMixer;
    
    public float transitionSpeed = 2f; // Adjust for smoothness
    public float intenseDuration = 5f; // Time before switching back to chill

    private Coroutine fadeCoroutine;
    private Coroutine countdownCoroutine;

    void Awake()
    {
        if (instance == null)
            instance = this;

        SwitchToChill();
    }

    public void BeIntense()
    {
        // Reset countdown timer
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = StartCoroutine(IntenseCountdown());

        // If we're already transitioning, stop it first
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeMusic("IntenseMusicVolume", "ChillMusicVolume", -20f, -80f));
    }

    private IEnumerator IntenseCountdown()
    {
        yield return new WaitForSeconds(intenseDuration);
        SwitchToChill(); // Switch back when the timer runs out
    }

    public void SwitchToChill()
    {
        // Stop any existing fade coroutine to prevent overlaps
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeMusic("ChillMusicVolume", "IntenseMusicVolume", -20f, -80f));
    }

    private IEnumerator FadeMusic(string fadeInParam, string fadeOutParam, float fadeInTarget, float fadeOutTarget)
    {
        float time = 0f;
        float currentFadeIn, currentFadeOut;
        audioMixer.GetFloat(fadeInParam, out currentFadeIn);
        audioMixer.GetFloat(fadeOutParam, out currentFadeOut);

        // Step 1: Fade in the new track
        while (time < 1f)
        {
            time += Time.deltaTime * transitionSpeed;
            audioMixer.SetFloat(fadeInParam, Mathf.Lerp(currentFadeIn, fadeInTarget, time));
            yield return null;
        }
        audioMixer.SetFloat(fadeInParam, fadeInTarget); // Ensure final value

        // Step 2: Ensure it's fully faded in before fading out the old track
        time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * transitionSpeed;
            audioMixer.SetFloat(fadeOutParam, Mathf.Lerp(currentFadeOut, fadeOutTarget, time));
            yield return null;
        }
        audioMixer.SetFloat(fadeOutParam, fadeOutTarget); // Ensure final value

        fadeCoroutine = null; // Mark the fade as completed
    }
}
