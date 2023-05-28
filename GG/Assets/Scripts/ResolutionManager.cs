using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    private Resolution resolution;

    private void Awake()
    {
        // Retrieve the resolution settings from player preferences
        int screenWidth = PlayerPrefs.GetInt("ScreenWidth", Screen.currentResolution.width);
        int screenHeight = PlayerPrefs.GetInt("ScreenHeight", Screen.currentResolution.height);
        bool isFullscreen = PlayerPrefs.GetInt("IsFullscreen", Screen.fullScreen ? 1 : 0) == 1;

        resolution = new Resolution
        {
            width = screenWidth,
            height = screenHeight,
            refreshRate = Screen.currentResolution.refreshRate
        };

        SetResolution(resolution.width, resolution.height, isFullscreen, resolution.refreshRate);
    }

    public void SetResolution(int width, int height, bool isFullscreen, int refreshRate)
    {
        resolution.width = width;
        resolution.height = height;
        resolution.refreshRate = refreshRate;

        Screen.SetResolution(width, height, isFullscreen, refreshRate);
    }

    private void OnApplicationQuit()
    {
        // Save the resolution settings to player preferences
        PlayerPrefs.SetInt("ScreenWidth", resolution.width);
        PlayerPrefs.SetInt("ScreenHeight", resolution.height);
        PlayerPrefs.SetInt("IsFullscreen", Screen.fullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}

