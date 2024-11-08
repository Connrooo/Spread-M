using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;
using JetBrains.Annotations;

public class SettingsMenu : MonoBehaviour
{

    PlayerStateMachine playerStateMachine;
    MenuManager menuManager;
    [Header("Resolution")]
    Resolution[] resolutions;
    List<Resolution> filteredResolutions;
    RefreshRate currentRefreshRate;
    int currentResIndex = 0;
    List<ResItem> finalResolutions = new();
    [SerializeField] TMP_Text resText;
    [Header("Sensitivity")]
    public float _SensitivityMultiplier = 1f;
    [Header("Post-Processing")]
    [SerializeField] private Volume postProcessingVolume;
    [Header("Post Processing Effects")]
    private ColorAdjustments colorAdjustments;

    [Header("PlayerPref Buttons,Sliders,Etc")]
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] Slider brightnessSlider;
    [SerializeField] Slider humanSlider;


    public float NumberOfHumans = 5;
    [SerializeField] TMP_Text humanText;

    

    private void Awake()
    {
        playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        menuManager = FindObjectOfType<MenuManager>();
        Screen.fullScreen = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResolutionStart();
        PostProcessingStart();
        LoadPlayerPrefs();
        LoadToggleValues();
    }

    private void LoadPlayerPrefs()
    {
        Screen.fullScreen = PlayerPrefs.GetInt("FullscreenValue") != 1;
        Debug.Log(PlayerPrefs.GetInt("FullscreenValue") != 0);
        _SensitivityMultiplier = PlayerPrefs.GetFloat("SensitivityValue", 1f);
        colorAdjustments.postExposure.value = PlayerPrefs.GetFloat("BrightnessValue", 0f);
        NumberOfHumans = PlayerPrefs.GetFloat("NumberOfHumans", 5f);
        Screen.SetResolution(PlayerPrefs.GetInt("HorResValue", Screen.currentResolution.width), PlayerPrefs.GetInt("VertResValue", Screen.currentResolution.height), Screen.fullScreen);
    }

    private void LoadToggleValues()
    {
        fullscreenToggle.isOn = Screen.fullScreen;
        sensitivitySlider.value = _SensitivityMultiplier;
        brightnessSlider.value = colorAdjustments.postExposure.value;
        humanSlider.value = NumberOfHumans;
        humanText.text = "Amount of Humans: " + NumberOfHumans;
        resText.text = resText.text = finalResolutions[currentResIndex].horizontal + "x" + finalResolutions[currentResIndex].vertical;
    }

    private void PostProcessingStart()
    {
        postProcessingVolume = GameObject.FindGameObjectWithTag("VolumeMain").GetComponent<Volume>();
        postProcessingVolume.profile.TryGet(out colorAdjustments);
    }
    private void ResolutionStart()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        currentRefreshRate = Screen.currentResolution.refreshRateRatio;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.value == currentRefreshRate.value)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            int resWidth = filteredResolutions[i].width;
            int resHeight = filteredResolutions[i].height;
            ResItem newResolution = new();
            newResolution.horizontal = resWidth;
            newResolution.vertical = resHeight;
            finalResolutions.Add(newResolution);
            if (i == filteredResolutions.Count - 1)
            {
                currentResIndex = i;
            }
        }
        Screen.SetResolution(finalResolutions[currentResIndex].horizontal, finalResolutions[currentResIndex].vertical, Screen.fullScreen);
    }
    public void MinusResolution()
    {
        currentResIndex--;
        if (currentResIndex < 0)
        {
            currentResIndex++;
            return;
        }
        ResolutionSetter();
    }
    public void PlusResolution()
    {
        currentResIndex++;
        if (currentResIndex > finalResolutions.Count - 1)
        {
            currentResIndex--;
            return;
        }
        ResolutionSetter();
    }

    private void ResolutionSetter()
    {
        Resolution resolution = new();
        resolution.width = finalResolutions[currentResIndex].horizontal;
        resolution.height = finalResolutions[currentResIndex].vertical;
        resText.text = finalResolutions[currentResIndex].horizontal + "x" + finalResolutions[currentResIndex].vertical;
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("HorResValue", resolution.width);
        PlayerPrefs.SetInt("VertResValue", resolution.height);
    }

    public void SetFullscreen(bool value)
    {
        Screen.fullScreen = value;
        PlayerPrefs.SetInt("FullscreenValue", (Screen.fullScreen ? 1 : 0));
    }
    public void SetSensitivity(float value)
    {
        _SensitivityMultiplier = value;
        PlayerPrefs.SetFloat("SensitivityValue", _SensitivityMultiplier);
    }   
    public void AdjustBrightness(float value)
    {
        colorAdjustments.postExposure.value= value;
        PlayerPrefs.SetFloat("BrightnessValue", colorAdjustments.postExposure.value);
    }

    public void HumanAmount(float value)
    {
        NumberOfHumans = value;
        PlayerPrefs.SetFloat("NumberOfHumans", NumberOfHumans);
        humanText.text = "Amount of Humans: " + NumberOfHumans;
    }

}


public class ResItem
{
    public int horizontal, vertical;
}
