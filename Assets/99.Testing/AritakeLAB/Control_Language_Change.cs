using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;

public class Control_Language_Change : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;
    public TMP_Text currentLanguageText;

    private int currentLocaleIndex;

    void Start()
    {
        currentLocaleIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);

        leftButton.onClick.AddListener(SelectPreviousLanguage);
        rightButton.onClick.AddListener(SelectNextLanguage);

        UpdateLanguageText();
    }

    void SelectPreviousLanguage()
    {
        currentLocaleIndex = (currentLocaleIndex - 1 + LocalizationSettings.AvailableLocales.Locales.Count) % LocalizationSettings.AvailableLocales.Locales.Count;
        ApplyLanguage();
    }

    void SelectNextLanguage()
    {
        currentLocaleIndex = (currentLocaleIndex + 1) % LocalizationSettings.AvailableLocales.Locales.Count;
        ApplyLanguage();
    }

    void ApplyLanguage()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[currentLocaleIndex];
        UpdateLanguageText();
    }

    void UpdateLanguageText()
    {
        if (currentLanguageText != null)
        {
            currentLanguageText.text = LocalizationSettings.SelectedLocale.LocaleName;
        }
    }
}
