using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    AudioMixer masterMixer;

    [SerializeField]
    TMPro.TMP_Dropdown resolutionDropdown;

    [SerializeField]
    GameObject pauseMenuUI;

    [SerializeField]
    GameObject optionsMenuUI;

    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolution = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolution;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetMasterVolume(float volume)
    {
        masterMixer.SetFloat("MasterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        masterMixer.SetFloat("MusicVolume", volume);
    }
    public void SetEnvironmentVolume(float volume)
    {
        masterMixer.SetFloat("EnvVolume", volume);
    }
    public void SetBattleVolume(float volume)
    {
        masterMixer.SetFloat("BattleVolume", volume);
    }
    public void SetGraphics(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void BackButton()
    {
        UIManager.Instance.ShowUI(UIType.Pause);
    }
}
