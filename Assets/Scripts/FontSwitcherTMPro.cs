
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class FontSwitcherTMPro : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TMP_FontAsset defaultFont;
    public TMP_FontAsset chineseFont;
    public TMP_FontAsset japaneseFont;
    public TMP_FontAsset russianFont;
   
    private void Awake()
    {
        if(text == null)
            text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if(text == null)
            text = GetComponent<TextMeshProUGUI>();
        
        PerformFontSwitch();
    }

    public void PerformFontSwitch()
    {
        if (text == null) return; 
        Locale currentLocale = LocalizationSettings.SelectedLocale;
        Debug.Log(currentLocale.SortOrder);
        if (currentLocale.SortOrder == 2)
            text.font = chineseFont;
        else if (currentLocale.SortOrder == 3)
            text.font = japaneseFont;
        else if (currentLocale.SortOrder == 5)
            text.font = russianFont;
        else
            text.font = defaultFont;
    }
}