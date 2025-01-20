using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OptionsHandler : MonoBehaviour
{
    public GameObject settingsUIObjects;
    public GameObject mainUIObjects; 
    public AudioMixer audioMixer;
    public Toggle fullscreenToggle;
    [FormerlySerializedAs("volumeToggle")] public Toggle musicVolumeToggle; 
    
    public void ChangeScreenMode(bool value)
    {
        if(fullscreenToggle.isOn)
            Screen.fullScreen = true;
        else 
            Screen.fullScreen = false;
    }

    public void ToggleSettingsMenu()
    {
        if (settingsUIObjects.activeSelf)
        {
            settingsUIObjects.SetActive(false);
            mainUIObjects.SetActive(true);
        }
        else
        {
            settingsUIObjects.SetActive(true);
            mainUIObjects.SetActive(false);
        }
    }

    public void ChangeMusic(bool value)
    {
        Debug.Log("Changing Music volume");
        
        if(musicVolumeToggle.isOn)
            UnmuteGroup();
        else
            MuteGroup();
    }
    
    public void UnmuteGroup()
    {
        Debug.Log("unmuting");
        audioMixer.SetFloat("Music", -20f);
    }
    
    public void MuteGroup()
    {
        Debug.Log("muting");
        audioMixer.SetFloat("Music", -80);
    }

}