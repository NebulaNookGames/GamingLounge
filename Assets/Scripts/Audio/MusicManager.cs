using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioMixer audioMixer;

    public float transitionSpeed = 2f; // Adjust for smoothness
    public float intenseDuration = 5f; // Time before switching back to chill
    public float chillVolume = -20f; // Chill music volume
    public float intenseVolume = -80f; // Intense music volume

    private Coroutine fadeCoroutine;
    private Coroutine countdownCoroutine;

    void Awake()
    {
        if (instance == null)
            instance = this;

        if (audioMixer == null)
        {
            Debug.LogError("AudioMixer is not assigned in the MusicManager.");
            return;
        }

        SwitchToChill();
    }

    public void BeIntense()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = StartCoroutine(IntenseCountdown());

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeMusic("IntenseMusicVolume", "ChillMusicVolume", chillVolume, intenseVolume));
    }

    private IEnumerator IntenseCountdown()
    {
        yield return new WaitForSeconds(intenseDuration);
        SwitchToChill();
    }

    public void SwitchToChill()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeMusic("ChillMusicVolume", "IntenseMusicVolume", chillVolume, intenseVolume));
    }

    private IEnumerator FadeMusic(string fadeInParam, string fadeOutParam, float fadeInTarget, float fadeOutTarget)
    {
        float time = 0f;
        float currentFadeIn, currentFadeOut;
        audioMixer.GetFloat(fadeInParam, out currentFadeIn);
        audioMixer.GetFloat(fadeOutParam, out currentFadeOut);

        while (time < 1f)
        {
            time += Time.deltaTime * transitionSpeed;
            audioMixer.SetFloat(fadeInParam, Mathf.Lerp(currentFadeIn, fadeInTarget, time));
            yield return null;
        }
        audioMixer.SetFloat(fadeInParam, fadeInTarget);

        time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * transitionSpeed;
            audioMixer.SetFloat(fadeOutParam, Mathf.Lerp(currentFadeOut, fadeOutTarget, time));
            yield return null;
        }
        audioMixer.SetFloat(fadeOutParam, fadeOutTarget);

        fadeCoroutine = null;
    }
}
