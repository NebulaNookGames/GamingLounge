using UnityEngine;

public class PlayOneShotRandomPitch : MonoBehaviour
{
    public AudioSource audioS;
    public float soundPitchVariation = 0.2f;  
    
    public void PlayOneShotWithRandomPitch(AudioClip audioC)
    {
        audioS.pitch = 1f + Random.Range(-soundPitchVariation, soundPitchVariation);
        audioS.PlayOneShot(audioC);
        Invoke(nameof(ResetPitch), audioC.length / audioS.pitch);
    }

    void ResetPitch()
    {
        audioS.pitch = 1f;
    }
}