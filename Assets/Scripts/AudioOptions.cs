using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("UI Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private const float MinDecibel = -80f;

    private void Start()
    {
        // Load and apply saved volumes
        InitializeSlider("MasterVolume", masterSlider);
        InitializeSlider("MusicVolume",  musicSlider);
        InitializeSlider("SFXVolume",    sfxSlider);

        // Subscribe to slider events
        masterSlider.onValueChanged.AddListener(v => SetVolume("MasterVolume", v));
        musicSlider.onValueChanged.AddListener(v => SetVolume("MusicVolume",  v));
        sfxSlider.onValueChanged.AddListener(v => SetVolume("SFXVolume",    v));
    }

    private void SetVolume(string parameterName, float sliderValue)
    {
        var dB = (sliderValue > 0f)
            ? Mathf.Log10(sliderValue) * 20f
            : MinDecibel;

        audioMixer.SetFloat(parameterName, dB);
        PlayerPrefs.SetFloat(parameterName, sliderValue);
    }
    
    private void InitializeSlider(string parameterName, Slider slider)
    {
        var savedValue = PlayerPrefs.GetFloat(parameterName, 0.75f);
        slider.value = savedValue;
        SetVolume(parameterName, savedValue);
    }
}