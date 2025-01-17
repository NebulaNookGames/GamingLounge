using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Localization.Settings;
using TMPro; 

public class LocaleSelector : MonoBehaviour
{
    private bool active = false;
    public TMP_Dropdown languageDropdown;
    
    IEnumerator SetLocale(int localeID)
    {
        active = true; 
        int currentSetLocale = languageDropdown.value;
        
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[currentSetLocale];
        active = false; 
    }

    public void ChangeLocale(int localeID)
    {
        if (active == true) return; 
        StartCoroutine(SetLocale(localeID));
    }
}