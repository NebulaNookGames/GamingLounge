using UnityEngine;

public class PlayOneShotRandomPitch : MonoBehaviour
{
    public AudioSource audioS;
    public float soundPitchVariation = 0.2f;  

    // Play audio clip with a random pitch variation
    public void PlayOneShotWithRandomPitch(AudioClip audioC)
    {
        if (audioS == null)
        {
            Debug.LogError("AudioSource not assigned.");
            return;
        }

        // Random pitch within the defined range
        audioS.pitch = 1f + Random.Range(-soundPitchVariation, soundPitchVariation);
        
        // Play the sound with randomized pitch
        audioS.PlayOneShot(audioC);

        // Reset pitch after the clip finishes
        Invoke(nameof(ResetPitch), audioC.length / audioS.pitch);
    }

    // Reset pitch to 1
    void ResetPitch()
    {
        audioS.pitch = 1f;
    }
}