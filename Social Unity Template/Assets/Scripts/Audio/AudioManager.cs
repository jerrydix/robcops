using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    
    public static AudioManager instance;
    
    public Sound[] musicSounds,sfxSounds;
    public AudioSource musicSource,sfxSource;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }
    
    private void Start()
    {
        //PlayMusic("Theme");
        if (GameManager.Instance.sfxOn)
        {
            sfxSource.mute = false;
        }
        else
        {
            sfxSource.mute = true;
        }
        
        sfxSource.volume = GameManager.Instance.sfxVolume;
    }
    
    

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            return;
        }

        musicSource.clip = s.Clip;
        musicSource.Play();
        
    }
    
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            return;
        }

        sfxSource.PlayOneShot(s.Clip);
        
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSfx()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void ButtonPressed()
    {
        PlaySFX("ButtonPressed");
    }

}