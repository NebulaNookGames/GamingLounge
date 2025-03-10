using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioS; // Reference to the AudioSource component
    [SerializeField] private AudioMixerGroup audioMixerGroup; // Output AudioMixerGroup
    
    private void Awake()
    {
        if (audioS == null)
        {
            Debug.LogError("AudioSource not assigned to AudioPlayer!");
            return;
        }
        
        if (audioMixerGroup != null)
        {
            audioS.outputAudioMixerGroup = audioMixerGroup;
        }
    }

    /// <summary>
    /// Plays an audio clip with a specific volume level.
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    /// <param name="volume">The volume at which to play the clip.</param>
    public void PlayAudioOneShot(AudioClip clip, float volume)
    {
        if (audioS != null)
        {
            audioS.PlayOneShot(clip, volume);
        }
    }

    /// <summary>
    /// Smoothly fades in the audio over a specified duration.
    /// </summary>
    /// <param name="targetVolume">The target volume to fade to.</param>
    /// <param name="fadeDuration">The duration over which to fade the audio.</param>
    private IEnumerator FadeInAudio(float targetVolume, float fadeDuration)
    {
        float currentTime = 0;
        float startVolume = audioS.volume;
        
        while (currentTime < fadeDuration)
        {
            audioS.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeDuration);
            currentTime += Time.deltaTime;
            yield return null;
        }
        
        audioS.volume = targetVolume; // Ensure it reaches the target volume
    }

    /// <summary>
    /// Starts fading in the audio with a specific volume and duration.
    /// </summary>
    /// <param name="volume">Target volume for the fade-in.</param>
    /// <param name="fadeDuration">Duration for the fade-in effect.</param>
    public void PlayAudioWithFadeIn(AudioClip clip, float volume, float fadeDuration)
    {
        PlayAudioOneShot(clip, volume);
        StartCoroutine(FadeInAudio(volume, fadeDuration));
    }
}
