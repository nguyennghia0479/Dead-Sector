using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private float minSliderValue = 1f;
    [SerializeField] private float maxSliderValue = 100f;

    [Header("Sensitivity Slider")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Text sensitivityValueText;

    [Header("Sounds Slider")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider soundsSlider;
    [SerializeField] private TMP_Text soundsValueText;
    [SerializeField] private string sfxParam;
    [SerializeField] private float decibelMultiplier = 40f;

    private const float MIN_SLIDER_VALUE = .0001f;
    private const string SFX_PARAM = "SFX_Setting";
    private const string SENSITIVITY_PARAM = "Sensitivity_Setting";

    private void OnEnable()
    {
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityValueChanged);
        soundsSlider.onValueChanged.AddListener(OnSoundsValueChanged);
        LoadSettings();
    }

    private void OnDisable()
    {
        sensitivitySlider.onValueChanged.RemoveListener(OnSensitivityValueChanged);
        soundsSlider.onValueChanged.RemoveListener(OnSoundsValueChanged);
        SaveSettings();
    }

    public void Initialized(PlayerCamera playerCamera)
    {
        sensitivitySlider.minValue = playerCamera.MinSensitivity;
        sensitivitySlider.maxValue = playerCamera.MaxSensitivity;
        sensitivitySlider.value = PlayerPrefs.GetFloat(SENSITIVITY_PARAM, playerCamera.DefaultSensitivity);

        OnSoundsValueChanged(soundsSlider.value);
    }

    private void OnSensitivityValueChanged(float newValue)
    {
        float valueNormalize = Mathf.InverseLerp(sensitivitySlider.minValue, sensitivitySlider.maxValue, newValue);
        float sliderValue = Mathf.Lerp(minSliderValue, maxSliderValue, valueNormalize);
        sensitivityValueText.text = Mathf.CeilToInt(sliderValue).ToString();
        UIEvents.RaiseSensitivitySliderChanaged(Mathf.Ceil(newValue));
    }

    private void OnSoundsValueChanged(float newValue)
    {
        float clampedValue = Mathf.Max(newValue, MIN_SLIDER_VALUE);
        float decibel = Mathf.Log10(clampedValue) * decibelMultiplier;
        audioMixer.SetFloat(sfxParam, decibel);

        float valueNormalize = Mathf.InverseLerp(soundsSlider.minValue, soundsSlider.maxValue, newValue);
        float sliderValue = Mathf.Lerp(minSliderValue, maxSliderValue, valueNormalize);
        soundsValueText.text = Mathf.CeilToInt(sliderValue).ToString();
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat(SENSITIVITY_PARAM, sensitivitySlider.value);
        PlayerPrefs.SetFloat(SFX_PARAM, soundsSlider.value);
    }

    public void LoadSettings()
    {
        sensitivitySlider.value = PlayerPrefs.GetFloat(SENSITIVITY_PARAM);
        soundsSlider.value = PlayerPrefs.GetFloat(SFX_PARAM, 1f);
    }
}
