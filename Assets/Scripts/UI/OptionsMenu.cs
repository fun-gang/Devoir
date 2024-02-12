using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown resolutionDropDown;
    Resolution[] resolutions;

    [HideInInspector] public static int gamepadDefault;
    public Animator gamepadCursorAnim;

    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRateRatio + "��";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();

        if (PlayerPrefs.HasKey("Gamepad")) {
            gamepadDefault = PlayerPrefs.GetInt("Gamepad");
        } 
        else {
            PlayerPrefs.SetInt("Gamepad", gamepadDefault);
        }
        ChangeGamepadDefaultUI();
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetGamepadType(int gamepadInd) {
        gamepadDefault = gamepadInd;
        PlayerPrefs.SetInt("Gamepad", gamepadDefault);
        ChangeGamepadDefaultUI();
    }

    private void ChangeGamepadDefaultUI() {
        if (gamepadDefault == 0) {
            gamepadCursorAnim.Play("PS");
        }
        else if (gamepadDefault == 1) {
            gamepadCursorAnim.Play("XB");
        }
    }
}
