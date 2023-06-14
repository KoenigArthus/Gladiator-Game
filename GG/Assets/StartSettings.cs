using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections.Generic;
using JSAM;

public class StartSettings : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider effectsSlider;
    public Slider musicSlider;
    public Toggle muteToggle;
    public Toggle modeToggle; // Toggle for selecting fullscreen/windowed mode
    public TMPro.TMP_Dropdown resolutionDropdown; // Dropdown for selecting resolution

    private float previousVolume;
    private float previousEffectsVolume;
    private float previousMusicVolume;

    private Resolution[] resolutions;

    private void Start()
    {
        // Add listeners to the sliders' value change events
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        effectsSlider.onValueChanged.AddListener(OnEffectsVolumeChanged);
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        // Add a listener to the toggle's value change event
        muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);

        // Add a listener to the mode toggle's value change event
        modeToggle.onValueChanged.AddListener(OnModeToggleChanged);

        // Find resolutions
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "hz";
            resolutionOptions.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.RefreshShownValue();

        // Load previously selected resolution or set it to the current resolution
        currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        resolutionDropdown.value = currentResolutionIndex;
        SetResolution(currentResolutionIndex);

        // Load saved settings or set default values
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        float savedEffectsVolume = PlayerPrefs.GetFloat("EffectsVolume", 1f);
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        bool savedIsMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        bool savedIsFullscreen = PlayerPrefs.GetInt("IsFullscreen", 1) == 1;
        int savedLanguageIndex = PlayerPrefs.GetInt("LanguageIndex", 0);

        // Apply the loaded settings
        volumeSlider.value = savedVolume;
        effectsSlider.value = savedEffectsVolume;
        musicSlider.value = savedMusicVolume;

        // Set the mute toggle state and apply volume accordingly
        muteToggle.isOn = savedIsMuted;
        OnMuteToggleChanged(savedIsMuted);

        // Set the mode toggle state
        modeToggle.isOn = savedIsFullscreen;
        OnModeToggleChanged(savedIsFullscreen);

        OnVolumeChanged(savedVolume);
        OnEffectsVolumeChanged(savedEffectsVolume);
        OnMusicVolumeChanged(savedMusicVolume);

        
    }

    private void OnResolutionChanged(int resolutionIndex)
    {
        SetResolution(resolutionIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);

        // Store the selected resolution index
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }

    private void OnVolumeChanged(float newVolume)
    {
        if (!muteToggle.isOn)
        {
            // Set the master volume using JSAM.AudioManager.SetMasterVolume
            AudioManager.SetMasterVolume(newVolume);
        }

        // Store the volume value
        PlayerPrefs.SetFloat("Volume", newVolume);
        PlayerPrefs.Save();
    }

    private void OnEffectsVolumeChanged(float newVolume)
    {
        if (!muteToggle.isOn)
        {
            // Set the effects volume using JSAM.AudioManager.SetEffectsVolume
            AudioManager.SetSoundVolume(newVolume);
        }

        // Store the effects volume value
        PlayerPrefs.SetFloat("EffectsVolume", newVolume);
        PlayerPrefs.Save();
    }

    private void OnMusicVolumeChanged(float newVolume)
    {
        if (!muteToggle.isOn)
        {
            // Set the music volume using JSAM.AudioManager.SetMusicVolume
            AudioManager.SetMusicVolume(newVolume);
        }

        // Store the music volume value
        PlayerPrefs.SetFloat("MusicVolume", newVolume);
        PlayerPrefs.Save();
    }

    private void OnMuteToggleChanged(bool isMuted)
    {
        if (isMuted)
        {
            // Store the previous volume values before muting
            previousVolume = volumeSlider.value;
            previousEffectsVolume = effectsSlider.value;
            previousMusicVolume = musicSlider.value;

            // Mute all sounds by setting volume to 0
            AudioManager.SetMasterVolume(0f);
            AudioManager.SetSoundVolume(0f);
            AudioManager.SetMusicVolume(0f);
        }
        else
        {
            // Restore the previous volume values after unmuting
            AudioManager.SetMasterVolume(previousVolume);
            AudioManager.SetSoundVolume(previousEffectsVolume);
            AudioManager.SetMusicVolume(previousMusicVolume);
        }

        // Store the mute state
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetWindowedMode(bool isWindowed)
    {
        // Set the application to run in windowed mode
        Screen.fullScreen = !isWindowed;

        // Store the fullscreen mode
        PlayerPrefs.SetInt("IsFullscreen", isWindowed ? 0 : 1);
        PlayerPrefs.Save();
    }

    private void OnModeToggleChanged(bool isFullscreen)
    {
        SetWindowedMode(!isFullscreen);
    }

    
}
