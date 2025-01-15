using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{ 
    [SerializeField] private AudioSource audioS;
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    
    private void Awake()
    {
        if (audioMixerGroup == null) return;
        audioS.outputAudioMixerGroup = audioMixerGroup;
    }

    public void PlayAudioOneShot(AudioClip clip)
    {
        audioS.PlayOneShot(clip);
    }
}
