using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioMixer audioMixer;
    public float transitionSpeed = 2f;
    public float intenseDuration = 5f;
    
    private float baseVolume = -20f;
    private bool isIntenseActive = false;
    private Coroutine fadeCoroutine;
    private Coroutine countdownCoroutine;

    void Awake()
    {
        if (instance == null)
            instance = this;
        SwitchToChill();
    }

    public void UpdateBaseVolume(float newBaseVolume)
    {
        baseVolume = newBaseVolume;

        // Apply the volume change to the currently active music
        if (isIntenseActive)
        {
            audioMixer.SetFloat("IntenseMusicVolume", baseVolume);
        }
        else
        {
            audioMixer.SetFloat("ChillMusicVolume", baseVolume);
        }
    }

    public void BeIntense()
    {
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);
        countdownCoroutine = StartCoroutine(IntenseCountdown());

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        isIntenseActive = true;
        fadeCoroutine = StartCoroutine(FadeMusic("IntenseMusicVolume", "ChillMusicVolume", baseVolume, -80));
    }

    private IEnumerator IntenseCountdown()
    {
        yield return new WaitForSeconds(intenseDuration);
        SwitchToChill();
    }

    public void SwitchToChill()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        isIntenseActive = false;
        fadeCoroutine = StartCoroutine(FadeMusic("ChillMusicVolume", "IntenseMusicVolume", baseVolume, -80));
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
            audioMixer.SetFloat(fadeOutParam, Mathf.Lerp(currentFadeOut, fadeOutTarget, time));
            yield return null;
        }
        audioMixer.SetFloat(fadeInParam, fadeInTarget);
        audioMixer.SetFloat(fadeOutParam, fadeOutTarget);

        fadeCoroutine = null;
    }
}
