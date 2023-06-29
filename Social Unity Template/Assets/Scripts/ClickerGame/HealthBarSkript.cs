using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSkript : MonoBehaviour
{
    [SerializeField] public Slider slider;

    public void setMaxHp(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }
    public void setHp(float hp)
    {
        slider.value = hp;
    }

}
