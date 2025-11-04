using System;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Slider sliderVolume, sliderSpeed, sliderSize;
    public Reader reader;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sliderVolume.value = PlayerPrefs.GetFloat("Volume");

        AudioListener.volume = PlayerPrefs.GetFloat("Volume");

        sliderVolume.onValueChanged.AddListener(SetVolume);

        sliderSpeed.value = PlayerPrefs.GetFloat("Speed");

        reader.speed = PlayerPrefs.GetFloat("Speed");

        sliderSpeed.onValueChanged.AddListener(SetSpeed);

        sliderSize.value = PlayerPrefs.GetInt("Size");

        reader.textMesh.fontSize = PlayerPrefs.GetFloat("Size") > 0 ? 32 : PlayerPrefs.GetFloat("Size");

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
        reader.speed = (0.1f - s) > 0.01f ? (0.1f - s) : 0.01f;
        PlayerPrefs.SetFloat("Speed", s);
        PlayerPrefs.Save();
    }

    public void SetSize(float s)
    {
        reader.textMesh.fontSize = s;
        PlayerPrefs.SetFloat("Size", s);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
