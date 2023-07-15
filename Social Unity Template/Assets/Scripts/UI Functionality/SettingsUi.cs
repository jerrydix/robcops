using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUi : MonoBehaviour
{
   [SerializeField] public Slider SFXVolume, MusicVolume;

   public void AdjustSFXVolume()
   {
      AudioManager.instance.ButtonVolume(SFXVolume.value);
   }
   public void AdjustMusicVolume()
   {
      AudioManager.instance.MusicVolume(MusicVolume.value);
   }
}
