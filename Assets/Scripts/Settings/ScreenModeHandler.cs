using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenModeHandler : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    public GameObject settingsMenu; // Reference to settings menu

    // Define the common resolutions you want to provide, including widescreen and ultrawide resolutions
    private Resolution[] commonResolutions = new Resolution[]
    {
        new Resolution { width = 1024, height = 576 },   // Lower res
        new Resolution { width = 1280, height = 720 },   // HD
        new Resolution { width = 1366, height = 768 },   // Standard HD
        new Resolution { width = 1600, height = 900 },   // HD+
        new Resolution { width = 1920, height = 1080 },  // Full HD
        new Resolution { width = 2560, height = 1080 },  // Ultrawide 1080p
        new Resolution { width = 2560, height = 1440 },  // Widescreen 1440p
        new Resolution { width = 3440, height = 1440 },  // Ultrawide 1440p
        new Resolution { width = 3840, height = 1600 },  // Ultrawide 4K
    };

    private bool isFullscreen;

    void Start()
    {
        settingsMenu.SetActive(true);  // Initially hide the settings menu

        LoadSettings(); // Load user preferences at start

        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.AddListener(ChangeResolution);

        settingsMenu.SetActive(false); // Show settings menu after initialization
    }

    private void OnDisable()
    {
        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.RemoveListener(SetFullscreen);

        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.RemoveListener(ChangeResolution);
    }

    private void OnEnable()
    {
        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
    }

    void LoadSettings()
    {
        // Load saved settings
        isFullscreen = PlayerPrefs.GetInt("FullScreen", 1) == 1;
        Screen.fullScreen = isFullscreen;

        if (fullscreenToggle != null)
            fullscreenToggle.isOn = isFullscreen;

        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions(); // Clear existing options in dropdown
            var options = new System.Collections.Generic.List<string>();

            // Sort the resolutions from low to high based on width*height
            var sortedResolutions = commonResolutions.OrderBy(r => r.width * r.height).ToArray();

            // Populate the dropdown with sorted resolutions
            for (int i = 0; i < sortedResolutions.Length; i++)
            {
                options.Add(sortedResolutions[i].width + "x" + sortedResolutions[i].height);
            }

            resolutionDropdown.AddOptions(options);

            // Get the current screen resolution and find the matching index in the dropdown
            Resolution currentRes = Screen.currentResolution;
            int selectedIndex = Array.FindIndex(sortedResolutions, r => r.width == currentRes.width && r.height == currentRes.height);

            // Set the dropdown to the currently active resolution
            resolutionDropdown.value = selectedIndex != -1 ? selectedIndex : 0; // Fallback to 0 if not found
            resolutionDropdown.RefreshShownValue();
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        // Ensure the screen mode switches between fullscreen and windowed
        this.isFullscreen = isFullscreen;
        Screen.fullScreen = isFullscreen; // This toggles the fullscreen state
    }

    public void ChangeResolution(int index)
    {
        // Sort the resolutions from low to high before setting the resolution
        var sortedResolutions = commonResolutions.OrderBy(r => r.width * r.height).ToArray();

        if (index >= 0 && index < sortedResolutions.Length)
        {
            Resolution res = sortedResolutions[index];

            // If the game is fullscreen, set to fullscreen
            if (Screen.fullScreen)
            {
                Screen.SetResolution(res.width, res.height, true);
            }
            else
            {
                // If in windowed mode, maintain aspect ratio
                float aspectRatio = (float)res.width / res.height;
                int windowedHeight = res.height;
                int windowedWidth = Mathf.RoundToInt(windowedHeight * aspectRatio);

                // Prevent the window from being resized to a non-proportional resolution
                if (windowedWidth < res.width) windowedWidth = res.width;

                Screen.SetResolution(windowedWidth, windowedHeight, false);
            }

            // Save the resolution index for next time
            PlayerPrefs.SetInt("ResolutionIndex", index);
            PlayerPrefs.Save();
        }
    }
}