using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider UISlider;

    private void Start()
    {
        SetMusicVolume();
        SetMasterVolume();
        SetSFXVolume();
        SetUIVolume();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
    }

    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        myMixer.SetFloat("master", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public void SetUIVolume()
    {
        float volume = UISlider.value;
        myMixer.SetFloat("UI", Mathf.Log10(volume) * 20);
    }
}
