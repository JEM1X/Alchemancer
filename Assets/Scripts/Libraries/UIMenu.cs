using UnityEngine;
using UnityEngine.Audio;
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
        settingsLabel.text = "Настройки";

        var masterSlider = UITK.AddElement<Slider>(settingsScreen, "masterSlider");
        masterSlider.value = PlayerPrefs.GetFloat(MASTER_PARAM, 0.5f);
        masterSlider.highValue = 1;
        masterSlider.label = "Общий";
        ApplyVolume(MASTER_PARAM, masterSlider.value, audioMixer);
        masterSlider.RegisterValueChangedCallback(evt => OnVolumeChanged(MASTER_PARAM, evt.newValue, audioMixer));

        var musicSlider = UITK.AddElement<Slider>(settingsScreen, "musicSlider");
        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_PARAM, 0.5f);
        musicSlider.highValue = 1;
        musicSlider.label = "Музыка";
        ApplyVolume(MUSIC_PARAM, musicSlider.value, audioMixer);
        musicSlider.RegisterValueChangedCallback(evt => OnVolumeChanged(MUSIC_PARAM, evt.newValue, audioMixer));

        var sfxSlider = UITK.AddElement<Slider>(settingsScreen, "sfxSlider");
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_PARAM, 0.5f);
        sfxSlider.highValue = 1;
        sfxSlider.label = "Звуки";
        ApplyVolume(SFX_PARAM, sfxSlider.value, audioMixer);
        sfxSlider.RegisterValueChangedCallback(evt => OnVolumeChanged(SFX_PARAM, evt.newValue, audioMixer));

        saveSettings = UITK.AddElement<Button>(settingsScreen, "saveSettings", "MainButton");
        saveSettings.text = "Сохранить";

        return settingsScreen;
    }

    private static void OnVolumeChanged(string paramName, float value, AudioMixer audioMixer)
    {
        ApplyVolume(paramName, value, audioMixer);
        PlayerPrefs.SetFloat(paramName, value);
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
}
