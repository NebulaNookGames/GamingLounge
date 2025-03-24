using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    public float baseVolume = -20; // Default base volume

    public float maxAmbientVolume;
    public float maxArcadeMachineVolume;
    public float maxMainMenuVolume;
    public float maxNPCVolume;
    public float maxOtherVolume;
    public float maxPlaceablesVolume;
    public float maxUIVolume;
    public float maxVFXVolume; 
    public float maxPlayerVolume; 
    
    private void Start()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        baseVolume = -musicSlider.value;
        ApplyBaseVolume();
    }
    
    public void SetMusicVolume(float value)
    {
        baseVolume = -value;
        ApplyBaseVolume();
    }

    public void SetSoundVolume()
    {
        float volume = -soundSlider.value; 
        
        if(volume > maxAmbientVolume)
            mixer.SetFloat("AmbientVolume", maxAmbientVolume);
        else
            mixer.SetFloat("AmbientVolume", volume);
        
        if(volume > maxMainMenuVolume)
            mixer.SetFloat("MainMenuVolume", maxMainMenuVolume);
        else
            mixer.SetFloat("MainMenuVolume", volume);
        
        if(volume > maxArcadeMachineVolume)
            mixer.SetFloat("ArcadeMachineVolume", maxArcadeMachineVolume);
        else
            mixer.SetFloat("ArcadeMachineVolume", volume);
        
        if(volume > maxNPCVolume)
            mixer.SetFloat("NPCVolume", maxNPCVolume);
        else
            mixer.SetFloat("NPCVolume", volume);
        
        if(volume > maxOtherVolume)
            mixer.SetFloat("OtherVolume", maxOtherVolume);
        else
            mixer.SetFloat("OtherVolume", volume);
        
        if(volume > maxPlaceablesVolume)
            mixer.SetFloat("PlaceablesVolume", maxPlaceablesVolume);
        else
            mixer.SetFloat("PlaceablesVolume", volume);
        
        if(volume > maxUIVolume)
            mixer.SetFloat("UIVolume", maxUIVolume);
        else
            mixer.SetFloat("UIVolume", volume);
        
        if(volume > maxVFXVolume)
            mixer.SetFloat("VFXVolume", maxVFXVolume);
        else
            mixer.SetFloat("VFXVolume", volume);
        
        if(volume > maxPlayerVolume)
            mixer.SetFloat("PlayerVolume", maxPlayerVolume);
        else
            mixer.SetFloat("PlayerVolume", volume);
    }

    private void ApplyBaseVolume()
    {
        if (MusicManager.instance != null)
        {
            MusicManager.instance.UpdateBaseVolume(baseVolume);
        }
    }
}