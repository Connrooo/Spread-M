using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

public class SettingsMenu : MonoBehaviour
{

    PlayerStateMachine playerStateMachine;
    MenuManager menuManager;
    [Header("Resolution")]
    [SerializeField] TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    List<Resolution> filteredResolutions;
    RefreshRate currentRefreshRate;
    int currentResIndex = 0;
    [Header("Sensitivity")]
    public float _SensitivityMultiplier = 1f;
    [Header("Post-Processing")]
    [SerializeField] private Volume postProcessingVolume;
    [Header("Post Processing Effects")]
    private ColorAdjustments colorAdjustments;
    private Vignette vignette;
    private FilmGrain filmGrain;
    private LiftGammaGain liftGammaGain;

    [Header("PlayerPref Buttons,Sliders,Etc")]
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] Slider brightnessSlider;
    [SerializeField] Slider contrastSlider;
    [SerializeField] Slider gammaSlider;
    [SerializeField] Toggle vignetteToggle;
    [SerializeField] Toggle filmGrainToggle;
    [SerializeField] Toggle subtitleToggle;


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
        //LoadPlayerPrefs();
    }


    private void loadToggleValues()
    {
        fullscreenToggle.isOn = Screen.fullScreen;
        sensitivitySlider.value = _SensitivityMultiplier;
        subtitleToggle.isOn = PlayerPrefs.GetInt("SubtitleToggle") != 0;
        brightnessSlider.value = colorAdjustments.postExposure.value;
        contrastSlider.value = colorAdjustments.contrast.value;
        gammaSlider.value = PlayerPrefs.GetFloat("GammaValue", 1f);
        vignetteToggle.isOn = vignette.active;
        filmGrainToggle.isOn = filmGrain.active;
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
        filteredResolutions.Reverse();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRateRatio;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.value == currentRefreshRate.value)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height;
            options.Add(resolutionOption);
            if (i == filteredResolutions.Count - 1)
            {
                currentResIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetResolution()
    {
        Resolution resolution = filteredResolutions[resolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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

    public void AdjustContrast(float value)
    {
        colorAdjustments.contrast.value = value*10;
        PlayerPrefs.SetFloat("ContrastValue", colorAdjustments.contrast.value);
    }
    public void AdjustGamma(float value)
    {
        liftGammaGain.gamma.value = new Vector4(value, value, value, value);
        PlayerPrefs.SetFloat("GammaValue", value);
    }
    public void ToggleVignette(bool value)
    {
        vignette.active = value;
        PlayerPrefs.SetInt("VignetteActive", (vignette.active ? 1 : 0));
    }
    public void ToggleFilmGrain(bool value)
    {
        filmGrain.active = value;
        PlayerPrefs.SetInt("FilmGrainActive", (filmGrain.active ? 1 : 0));
    }

    public void HumanAmount(float value)
    {
        NumberOfHumans = value;
        PlayerPrefs.SetFloat("NumberOfHumans", NumberOfHumans);
        humanText.text = "Amount of Humans: " + NumberOfHumans;
    }

}
