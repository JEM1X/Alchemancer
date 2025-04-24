using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

public static class UIMenu
{
    private const string MASTER_PARAM = "MasterVolume";
    private const string MUSIC_PARAM = "MusicVolume";
    private const string SFX_PARAM = "SFXVolume";

    public static VisualElement InitSettingsMenu(AudioMixer audioMixer, out Button saveSettings)
    {
        var settingsScreen = UITK.CreateElement("settingsScreen", "InGameFrame");
        settingsScreen.style.position = Position.Absolute;

        var settingsLabel = UITK.AddElement<Label>(settingsScreen, "settingsLabel", "MainText");
        UITK.LocalizeStringUITK(settingsLabel, UITK.UITABLE, "Settings.Label");

        var masterSlider = UITK.AddElement<Slider>(settingsScreen, "masterSlider");
        masterSlider.value = PlayerPrefs.GetFloat(MASTER_PARAM, 0.5f);
        masterSlider.highValue = 1;
        masterSlider.label = " ";
        masterSlider.RegisterValueChangedCallback(evt => OnVolumeChanged(MASTER_PARAM, evt.newValue, audioMixer));
        ApplyVolume(MASTER_PARAM, masterSlider.value, audioMixer);
        UITK.LocalizeStringUITK(masterSlider.labelElement, UITK.UITABLE, "Settings.Master");

        var musicSlider = UITK.AddElement<Slider>(settingsScreen, "musicSlider");
        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_PARAM, 0.5f);
        musicSlider.highValue = 1;
        musicSlider.label = " ";
        musicSlider.RegisterValueChangedCallback(evt => OnVolumeChanged(MUSIC_PARAM, evt.newValue, audioMixer));
        ApplyVolume(MUSIC_PARAM, musicSlider.value, audioMixer);
        UITK.LocalizeStringUITK(musicSlider.labelElement, UITK.UITABLE, "Settings.Music");

        var sfxSlider = UITK.AddElement<Slider>(settingsScreen, "sfxSlider");
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_PARAM, 0.5f);
        sfxSlider.highValue = 1;
        sfxSlider.label = " ";
        sfxSlider.RegisterValueChangedCallback(evt => OnVolumeChanged(SFX_PARAM, evt.newValue, audioMixer));
        ApplyVolume(SFX_PARAM, sfxSlider.value, audioMixer);
        UITK.LocalizeStringUITK(sfxSlider.labelElement, UITK.UITABLE, "Settings.Sound");

        var languageDropdown = UITK.AddElement<DropdownField>(settingsScreen, "languageDropdown");
        SetupLanguageDropdown(languageDropdown);

        saveSettings = UITK.AddElement<Button>(settingsScreen, "saveSettings", "MainButton");
        UITK.LocalizeStringUITK(saveSettings, UITK.UITABLE, "Settings.Save");
        saveSettings.clicked += PlayerPrefs.Save;

        return settingsScreen;
    }

    private static void OnVolumeChanged(string paramName, float value, AudioMixer audioMixer)
    {
        ApplyVolume(paramName, value, audioMixer);
        PlayerPrefs.SetFloat(paramName, value);
    }

    private static void SetupLanguageDropdown(DropdownField languageDropdown)
    {
        LoadLocale();

        var availableLocales = LocalizationSettings.AvailableLocales.Locales;

        List<string> localeNames = new();
        var currentLocaleIndex = 0;

        for (int i = 0; i < availableLocales.Count; i++)
        {
            var locale = availableLocales[i];
            string label = locale.Identifier.CultureInfo.NativeName;
            localeNames.Add(label);

            if (LocalizationSettings.SelectedLocale == locale)
                currentLocaleIndex = i;
        }

        languageDropdown.choices = localeNames;
        languageDropdown.index = currentLocaleIndex;

        languageDropdown.RegisterValueChangedCallback(evt =>
        {
            int selectedIndex = languageDropdown.index;
            if (selectedIndex >= 0 && selectedIndex < availableLocales.Count)
            {
                LocalizationSettings.SelectedLocale = availableLocales[selectedIndex];

                PlayerPrefs.SetString("SelectedLocale", availableLocales[selectedIndex].Identifier.Code);
            }
        });
    }

    private static void LoadLocale()
    {
        string savedCode = PlayerPrefs.GetString("SelectedLocale", "en");
        
        Debug.Log(savedCode);
        var savedLocale = LocalizationSettings.AvailableLocales.GetLocale(savedCode);
        if (savedLocale != null)
        {
            LocalizationSettings.SelectedLocale = savedLocale;
        }
    }

    private static void ApplyVolume(string paramName, float value, AudioMixer audioMixer)
    {
        float volumeDb = Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20f;
        audioMixer.SetFloat(paramName, volumeDb);
    }

    public static void ToggleScreen(VisualElement element, ref bool trigger)
    {
        if (trigger)
        {
            element.style.display = DisplayStyle.None;
            trigger = false;
        }
        else
        {
            element.style.display = DisplayStyle.Flex;
            trigger = true;
        }
    }

    public static Label AddHintBox(VisualElement element)
    {
        var hintBox = UITK.AddElement<Label>(element, "HintBox", "SubText");
        hintBox.pickingMode = PickingMode.Ignore;
        hintBox.BringToFront();

        element.RegisterCallback<PointerEnterEvent>(evt =>
        {
            hintBox.style.display = DisplayStyle.Flex;
        });

        element.RegisterCallback<PointerLeaveEvent>(evt =>
        {
            hintBox.style.display = DisplayStyle.None;
        });

        return hintBox;
    }

    public static Label AddHintBox(VisualElement element, string hint)
    {
        var hintBox = AddHintBox(element);
        hintBox.text = hint;

        return hintBox;
    }

    public static Label AddLocalizedHintBox(VisualElement element, string table, string key)
    {
        var hintBox = AddHintBox(element);
        UITK.LocalizeStringUITK(hintBox, table, key);

        return hintBox;
    }
}
