using UnityEngine;

public class PlayRandomAudio : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    
    // Play a random audio clip
    public void PlayRandom()
    {
        // Check if the audio source or clips array is null or empty
        if (audioSource == null || audioClips.Length == 0)
        {
            Debug.LogError("AudioSource or AudioClips not assigned.");
            return;
        }

        // Pick a random index
        int randomIndex = Random.Range(0, audioClips.Length);
        
        // Play the selected random clip
        audioSource.PlayOneShot(audioClips[randomIndex]);
    }
}