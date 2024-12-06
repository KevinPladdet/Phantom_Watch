using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{

    [SerializeField] private Slider musicSlider;
    [SerializeField] private float musicSliderValue;

    [SerializeField] private Slider sfxSlider;
    [SerializeField] private float sfxSliderValue;

    public void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicSliderValue");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxSliderValue");
    }

    public void ChangeMusicSlider(float musicValue)
    {
        musicSliderValue = musicValue;
        PlayerPrefs.SetFloat("musicSliderValue", musicSliderValue);
    }

    public void ChangeSFXSlider(float sfxValue)
    {
        sfxSliderValue = sfxValue;
        PlayerPrefs.SetFloat("sfxSliderValue", sfxSliderValue);
    }
}