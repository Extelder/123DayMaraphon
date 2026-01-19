using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;

public class LanguageSelectionMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown languageDropdown;

    private const string PREFS_KEY = "selected_locale";

    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;

        PopulateDropdown();

        LoadSavedLanguage();
        
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
    }

    private void PopulateDropdown()
    {
        languageDropdown.ClearOptions();

        var options = new List<string>();
        var locales = LocalizationSettings.AvailableLocales.Locales;

        foreach (var locale in locales)
            options.Add(locale.LocaleName);

        languageDropdown.AddOptions(options);
    }

    private void LoadSavedLanguage()
    {
        if (PlayerPrefs.HasKey(PREFS_KEY))
        {
            int savedIndex = PlayerPrefs.GetInt(PREFS_KEY);
            savedIndex = Mathf.Clamp(savedIndex, 0, LocalizationSettings.AvailableLocales.Locales.Count - 1);
            languageDropdown.value = savedIndex;
            SetLocale(savedIndex);
        }
        else
        {
            var current = LocalizationSettings.SelectedLocale;
            var index = LocalizationSettings.AvailableLocales.Locales.IndexOf(current);
            if (index >= 0)
                languageDropdown.value = index;
        }
    }

    private void OnLanguageChanged(int index)
    {
        SetLocale(index);
        PlayerPrefs.SetInt(PREFS_KEY, index);
        PlayerPrefs.Save();
    }

    private void SetLocale(int index)
    {
        Locale locale = LocalizationSettings.AvailableLocales.Locales[index];
        LocalizationSettings.SelectedLocale = locale;
    }
}
