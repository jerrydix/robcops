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
        }
        else
        {
            sfxVolumeButton.GetComponent<Image>().color = Color.red;
        }
    }

    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(musicVolumeSlider.GetComponent<Slider>().value);
    }

    public void SfxVolume()
    {
        AudioManager.instance.SfxVolume(sfxVolumeSlider.GetComponent<Slider>().value);
    }
    
    
    

    // Start is called before the first frame update
    /*public void MusicSwitch()
    {
        
        public Button musicVolumeButtom;
            if (musicMuted == true)
        {
            musicVolumeSlider.SetActive(false);
            musicVolumeButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            musicVolumeSlider.SetActive(true);
            musicVolumeButton.GetComponent<Image>().color = Color.white;
        }
    }
    public void SoundSwitch()
    {
        soundMuted = !soundMuted;
        if (soundMuted == true)
        {
            soundVolumeSlider.SetActive(false);
            soundVolumeButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            soundVolumeSlider.SetActive(true);
            soundVolumeButton.GetComponent<Image>().color = Color.white;
        }
    }*/
}