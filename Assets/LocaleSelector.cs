using UnityEngine;
using System.Collections;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour
{
    private bool active = false; 
    
    IEnumerator SetLocale(int localeID)
    {
        active = true; 
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        active = false; 
    }

    public void ChangeLocale(int localeID)
    {
        if (active == true) return; 
        StartCoroutine(SetLocale(localeID));
    }
}