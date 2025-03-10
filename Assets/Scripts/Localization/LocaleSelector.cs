using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Localization.Settings;
using TMPro;

public class LocaleSelector : MonoBehaviour
{
    #region Serialized Fields

    // Dropdown UI element for selecting language
    public TMP_Dropdown languageDropdown;

    #endregion

    #region Private Variables

    // Singleton instance of LocaleSelector
    public static LocaleSelector Instance;

    // Flag to prevent multiple locale changes in quick succession
    private bool active = false;

    #endregion

    #region Unity Methods

    // Called when the script is initialized
    private void Awake()
    {
        // Ensure the singleton instance is set up
        if (Instance == null)
            Instance = this;
    }

    #endregion

    #region Locale Management

    // Coroutine to set the selected locale
    private IEnumerator SetLocale(int localeID)
    {
        // Prevent changing locale if it's already in progress
        active = true;
        
        // Store the current dropdown value (language selection)
        int currentSetLocale = languageDropdown.value;

        // Wait until localization settings are initialized
        yield return LocalizationSettings.InitializationOperation;

        // Set the selected locale based on the dropdown value
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[currentSetLocale];

        // Reset the active flag after the locale change
        active = false;
    }

    // Method to handle locale change triggered by the dropdown
    public void ChangeLocale(int localeID)
    {
        // Return early if a locale change is already in progress
        if (active == true) return;

        // Start the coroutine to change the locale
        StartCoroutine(SetLocale(localeID));
    }

    #endregion
}