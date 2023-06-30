using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickerGameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button safe;
    [SerializeField] private TextMeshProUGUI clickDamageMultiplierText;

    private float _clickDamageMultiplier;
    [HideInInspector] public float currentSafeHealth;
    [HideInInspector] public float maxSafeHealth;
    
    [HideInInspector] public int safeLevel = 1;
    
    [SerializeField] public Sprite[] safeSprites;
    [SerializeField] private Image currentImage;

    [SerializeField] private Animator animator;

    [SerializeField] public HealthBar hpBar;
    // Start is called before the first frame update
    void Start()
    {
        _clickDamageMultiplier = 1; //get from GameManager
        maxSafeHealth = 100;
        currentSafeHealth = maxSafeHealth; //get from gameManager according to level
        currentImage.sprite = safeSprites[safeLevel + 1];
        hpBar.setMaxHp(currentSafeHealth);
        hpBar.setHp(currentSafeHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (hpBar.slider.value == 0)
        {
            //Todo: win screen
        }
    }

    public void DamageSafe()
    {
        if (currentSafeHealth != 0)
        {
            currentSafeHealth -= 1 * _clickDamageMultiplier;
            animator.SetTrigger("ClickTrigger");
            hpBar.setHp(currentSafeHealth);
        }
        //todo Instantiate sprite for click or wobble safe (visual click feedback)
        
    }
}
