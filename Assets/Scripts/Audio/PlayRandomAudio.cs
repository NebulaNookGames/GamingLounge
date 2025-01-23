using UnityEngine;

public class PlayRandomAudio : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    
    public void PlayRandom()
    {
        int randomIndex = Random.Range(0, audioClips.Length);
        audioSource.PlayOneShot(audioClips[randomIndex]);
    }
}
