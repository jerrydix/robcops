using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 100f)] public float volume;
    [Range(0f, 100f)] public float pitch;

    public bool loop;
    
    [HideInInspector] public AudioSource source;


    public void setVolume(float n)
    {
        volume = n;
    }
}