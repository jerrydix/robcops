using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] public Slider slider;
    
    /*
     * How to use the HealthBar Script:
     * First add the HpBarUI Prefab to the scene
     * In the UIManager:
     * Add a serializedField to the szene and assign the preset to it
     * Set the starting Value of the hpBar with setMaxHp() and setHp()
     * After that create a Method that influences the currentHp via setHp()
     * Eat a cookie (important!!!)
     */

    public void setMaxHp(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }
    public void setHp(float hp)
    {
        slider.value = hp;
    }

}
