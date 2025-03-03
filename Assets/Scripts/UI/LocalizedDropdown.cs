using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class LocalizedDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public List<LocalizedString> localizedOptions;
    public LocalizedString loadingText; // Localized "Loading..." text

    private void Start()
    {
        UpdateDropdownOptions();
        LocalizationSettings.SelectedLocaleChanged += _ => UpdateDropdownOptions();
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

        // Restore previous selection (ensure index is within valid range)
        dropdown.value = Mathf.Clamp(previousIndex, 0, dropdown.options.Count - 1);
    }
}
