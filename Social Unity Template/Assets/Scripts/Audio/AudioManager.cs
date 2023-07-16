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
        PlayMusic("Theme");
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
/*public static AudioManager instance;

public bool soundMuted;
public bool musicMuted;


[SerializeField] public GameObject soundVolumeSlider;
[SerializeField] public GameObject musicVolumeSlider;
[SerializeField] public GameObject soundVolumeButton;
[SerializeField] public GameObject musicVolumeButton;

[SerializeField] public GameObject SettingsUI;


public void Awake()
{
    if (instance == null)
    {
        instance = this;
    }
    else
    {
        Destroy(gameObject);
        return;
    }

    DontDestroyOnLoad(gameObject);

    musicMuted = false;
    soundMuted = false;
    
    foreach (var s in sounds)
    {
        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
    }
    soundVolumeSlider.GetComponent<Slider>().value = Array.Find(sounds, sound => sound.name == "ButtonPressed").volume;
    musicVolumeSlider.GetComponent<Slider>().value = Array.Find(sounds, sound => sound.name == "Theme").volume;
}

private void Start()
{
    Play("Theme");
}

private void Update()
{
    Array.Find(sounds, sound => sound.name == "ButtonPressed").SetVolume(soundVolumeSlider.GetComponent<Slider>().value) ;
        Array.Find(sounds, sound => sound.name == "Theme").volume = musicVolumeSlider.GetComponent<Slider>().value;
        foreach (var s in sounds)
        {
            s.source.volume = s.volume;
            //s.source.pitch = s.pitch;
            //s.source.loop = s.loop;
        }
}

public void Play(string name)
{
    Sound s = Array.Find(sounds, sound => sound.name == name);
    if (s == null)
    {
        return;
    }

    s.source.Play();
    
}

public void ButtonPressedSound()
{
    if (soundMuted == false)
    {
        Play("ButtonPressed");
    }
}

public void MusicSwitch()
{
    musicMuted = !musicMuted;
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
}

}*/