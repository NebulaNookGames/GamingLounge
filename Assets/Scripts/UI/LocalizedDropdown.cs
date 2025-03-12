using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class LocalizedDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public List<LocalizedString> localizedOptions;
    public LocalizedString loadingText; // Localized "Loading..." text

    // Fonts for different locales
    public TMP_FontAsset japaneseFont; 
    public TMP_FontAsset simplifiedChineseFont;
    public TMP_FontAsset russianFont;
    public TMP_FontAsset defaultFont;
    
    private void Start()
    {
        // Initialize dropdown options
        UpdateDropdownOptions();

        // Listen for changes in dropdown selection
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        // Update options if locale changes
        LocalizationSettings.SelectedLocaleChanged += _ => UpdateDropdownOptions();
    }

    private void OnDropdownValueChanged(int index)
    {
        // Get the selected locale
        int currentSortOrder = LocalizationSettings.SelectedLocale.SortOrder;

        // Check if the selected option is Simplified Chinese
        if (currentSortOrder == 2) // Simplified Chinese
        {
            // Set fonts to Simplified Chinese font
            SetFonts(simplifiedChineseFont);
        }
        else
        {
            // Set fonts to Default font
            SetFonts(defaultFont);
        }
    }

    private void SetFonts(TMP_FontAsset font)
    {
        // Change the font of the selected label (the selected option)
        TMP_Text selectedText = dropdown.captionText;  // The text of the currently selected item
        if (selectedText != null)
        {
            selectedText.font = font;
        }

        // After the options have been initialized, change the font of all options
        foreach (Transform optionTransform in dropdown.transform)
        {
            // Ensure the option is one of the instantiated options (not the dropdown root)
            if (optionTransform.name.Contains("Item"))
            {
                TMP_Text optionText = optionTransform.GetComponentInChildren<TMP_Text>();
                if (optionText != null)
                {
                    optionText.font = font;
                }
            }
        }
    }

    private void UpdateDropdownOptions()
    {
        int previousIndex = dropdown.value; // Store current selection
        dropdown.ClearOptions();
        StartCoroutine(LoadLocalizedOptions(previousIndex));
    }

    private IEnumerator LoadLocalizedOptions(int previousIndex)
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        // Fetch localized "Loading..." text
        var loadingHandle = loadingText.GetLocalizedStringAsync();
        yield return loadingHandle;
        string loadingString = loadingHandle.Result; // Get localized Loading text

        // Initialize dropdown with localized loading placeholders
        for (int i = 0; i < localizedOptions.Count; i++)
        {
            options.Add(new TMP_Dropdown.OptionData(loadingString));
        }

        dropdown.options = options;
        dropdown.RefreshShownValue();

        // Load the localized options
        for (int i = 0; i < localizedOptions.Count; i++)
        {
            int index = i;
            var handle = localizedOptions[index].GetLocalizedStringAsync();
            yield return handle; // Wait for translation to load

            if (index < options.Count)
            {
                options[index].text = handle.Result;
            }
        }

        dropdown.options = options;
        dropdown.RefreshShownValue();

        // Update the fonts based on the current locale
        int currentSortOrder = LocalizationSettings.SelectedLocale.SortOrder;
        if (currentSortOrder == 2) // Simplified Chinese
        {
            SetFonts(simplifiedChineseFont);
        }
        else if (currentSortOrder == 3)
        {
            SetFonts(japaneseFont);
        }
        else if (currentSortOrder == 5)
        {
            SetFonts(russianFont);
        }
        else
        {
            SetFonts(defaultFont);
        }

        // Restore previous selection (ensure index is within valid range)
        dropdown.value = Mathf.Clamp(previousIndex, 0, dropdown.options.Count - 1);
    }
}
