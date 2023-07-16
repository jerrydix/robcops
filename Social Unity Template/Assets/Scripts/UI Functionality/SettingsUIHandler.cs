using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class SettingsUIHandler : MonoBehaviour
{
    [SerializeField] public GameObject musicVolumeButton;
    [SerializeField] public GameObject sfxVolumeButton;

    [SerializeField] public GameObject musicVolumeSlider;
    [SerializeField] public GameObject sfxVolumeSlider;

    private void Awake()
    {
        musicVolumeButton.GetComponent<Image>().color = Color.white;
        sfxVolumeButton.GetComponent<Image>().color = Color.white;
    }

    private void Start() //setzen
    {
        sfxVolumeSlider.GetComponent<Slider>().value = GameManager.Instance.sfxVolume;
        if (GameManager.Instance.sfxOn)
        {
            sfxVolumeSlider.SetActive(true);
            sfxVolumeButton.GetComponent<Image>().color = Color.white;
        }
        else
        {
            sfxVolumeSlider.SetActive(false);
            sfxVolumeButton.GetComponent<Image>().color = Color.red;
        }
    }


    public void MusicSwitch()
    {
        AudioManager.instance.ToggleMusic();
        musicVolumeSlider.SetActive(!musicVolumeSlider.activeInHierarchy);
        if (musicVolumeButton.GetComponent<Image>().color == Color.red)
        {
            musicVolumeButton.GetComponent<Image>().color = Color.white;
        }
        else
        {
            musicVolumeButton.GetComponent<Image>().color = Color.red;
        }
    }
    public void SfxSwitch()
    {
        AudioManager.instance.ToggleSfx();
        sfxVolumeSlider.SetActive(!sfxVolumeSlider.activeInHierarchy);
        if (sfxVolumeButton.GetComponent<Image>().color == Color.red)
        {
            sfxVolumeButton.GetComponent<Image>().color = Color.white;
            GameManager.Instance.sfxOn = true; //setzen isOn
        }
        else
        {
            sfxVolumeButton.GetComponent<Image>().color = Color.red;
            GameManager.Instance.sfxOn = false; //setzen isOn
        }
    }

    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(musicVolumeSlider.GetComponent<Slider>().value);
    }

    public void SfxVolume()
    {
        GameManager.Instance.sfxVolume = sfxVolumeSlider.GetComponent<Slider>().value; //setzen volume
        AudioManager.instance.SfxVolume(sfxVolumeSlider.GetComponent<Slider>().value);
    }
    
}