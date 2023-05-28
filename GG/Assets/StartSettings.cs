using UnityEngine;
using UnityEngine.UI;
using JSAM;
using System.Collections.Generic;

public class StartSettings : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider effectsSlider;
    public Slider musicSlider;
    public Toggle muteToggle;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public Toggle modeToggle; // Toggle for selecting fullscreen/windowed mode

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

        // Add listener to resolution dropdown value change event
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
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
    }

    private void OnEffectsVolumeChanged(float newVolume)
    {
        if (!muteToggle.isOn)
        {
            // Set the effects volume using JSAM.AudioManager.SetEffectsVolume
            AudioManager.SetSoundVolume(newVolume);
        }
    }

    private void OnMusicVolumeChanged(float newVolume)
    {
        if (!muteToggle.isOn)
        {
            // Set the music volume using JSAM.AudioManager.SetMusicVolume
            AudioManager.SetMusicVolume(newVolume);
        }
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
    }

    public void SetWindowedMode(bool isWindowed)
    {
        // Set the application to run in windowed mode
        Screen.fullScreen = !isWindowed;
    }

    private void OnModeToggleChanged(bool isFullscreen)
    {
        SetWindowedMode(!isFullscreen);
    }
}
