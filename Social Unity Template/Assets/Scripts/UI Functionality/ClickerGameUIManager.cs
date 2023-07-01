using System;
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

    [SerializeField] public GameObject winScreen;

    private float startTime;

    private float finalTime;

    private String[] players;
    // Start is called before the first frame update
    void Start()
    {
        _clickDamageMultiplier = 1; //get from GameManager
        maxSafeHealth = 2;
        currentSafeHealth = maxSafeHealth; //get from gameManager according to level
        currentImage.sprite = safeSprites[safeLevel + 1];
        //HealthBar Setup
        hpBar.setMaxHp(currentSafeHealth);
        hpBar.setHp(currentSafeHealth);
        
        //Win Screen Setup
        winScreen.SetActive(false);
        startTime = Time.time;
        finalTime = 0;
        players = new string[5];
        players[0] = "KFCGuru"; //todo get Player names

        //todo get safeMoney
    }

    // Update is called once per frame
    void Update()
    {
        if (hpBar.slider.value == 0)
        {
            if (finalTime == 0)
            {
                finalTime = Time.time - startTime;
                winScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("In " + finalTime.ToString().Substring(
                    0,5) + " Seconds");
                String participants = "The Crew: ";
                foreach (var p in players)
                {
                    participants += "\n" + p;
                }
                winScreen.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText(participants);
                String reward = "Your Loot: $$$$$$$";
                winScreen.transform.GetChild(4).GetComponent<TextMeshProUGUI>().SetText(reward);
            }

            winScreen.SetActive(true);
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
