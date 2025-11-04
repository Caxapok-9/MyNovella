using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public Slider sliderVolume, sliderSpeed, sliderSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sliderVolume.value = PlayerPrefs.GetFloat("Volume");

        AudioListener.volume = PlayerPrefs.GetFloat("Volume");

        sliderVolume.onValueChanged.AddListener(SetVolume);

        sliderSpeed.value = PlayerPrefs.GetFloat("Speed");

        sliderSpeed.onValueChanged.AddListener(SetSpeed);

        sliderSize.value = PlayerPrefs.GetInt("Size");

        sliderSize.onValueChanged.AddListener(SetSize);
    }

    public void SetVolume(float vol)
    {
        AudioListener.volume = vol;
        PlayerPrefs.SetFloat("Volume", vol);
        PlayerPrefs.Save();
    }

    public void SetSpeed(float s)
    {
        PlayerPrefs.SetFloat("Speed", s);
        PlayerPrefs.Save();
    }

    public void SetSize(float s)
    {
        PlayerPrefs.SetFloat("Size", s);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
