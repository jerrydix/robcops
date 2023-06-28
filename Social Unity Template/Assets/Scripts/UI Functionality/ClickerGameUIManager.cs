using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickerGameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Slider safeHealth;
    [SerializeField] private Button safe;
    [SerializeField] private TextMeshProUGUI clickDamageMultiplierText;

    private float _clickDamageMultiplier;
    [HideInInspector] public float currentSafeHealth;
    [HideInInspector] public int safeLevel = 1;
    
    [SerializeField] public Sprite[] safeSprites;
    [SerializeField] private Image currentImage;

    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        _clickDamageMultiplier = 1; //get from GameManager
        currentSafeHealth = 100; //get from gameManager according to level
        currentImage.sprite = safeSprites[safeLevel + 1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageSafe()
    {
        currentSafeHealth -= 1 * _clickDamageMultiplier;
        animator.SetTrigger("ClickTrigger");
        //todo Instantiate sprite for click or wobble safe (visual click feedback)
    }
}
