using System;
using System.Collections;
using Connections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickerGameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button safe;
    [SerializeField] private TextMeshProUGUI clickDamageMultiplierText;

    [SerializeField] public Sprite[] safeSprites;
    [SerializeField] private Image currentImage;

    [SerializeField] private Animator animator;

    [SerializeField] public HealthBar hpBar;

    [SerializeField] public GameObject winScreen;
    [SerializeField] public GameObject lostScreen;

    [SerializeField] private TextMeshProUGUI timerTextWinScreen;
    [SerializeField] private TextMeshProUGUI remainingSafeHP;
    [SerializeField] private TextMeshProUGUI moneyGain;
    [SerializeField] private TextMeshProUGUI moneyLose;
    [SerializeField] private TextMeshProUGUI totalMoneyWinScreen;
    [SerializeField] private TextMeshProUGUI totalMoneyLostScreen;


    private float _clickDamageMultiplier;
    private int _currentSafeHealth;

    private string currentTakenTime; 

    private string currentTime;

    private float finalTime;
    private bool gameComplete;

    private string[] players;

    private float startTime;

    private bool timeOver;

    // Start is called before the first frame update
    private void Start()
    {
        timeOver = false;
        gameComplete = false;
        _clickDamageMultiplier = 1; //get from GameManager
        _currentSafeHealth = GameManager.Instance.currentHP; //get from gameManager according to level
        currentImage.sprite = safeSprites[GameManager.Instance.currentSafeLevel - 1];

        timerText.text = GameManager.Instance.currentMinutes + ":" + GameManager.Instance.currentSeconds;

        //HealthBar Setup
        hpBar.setMaxHp(_currentSafeHealth);
        hpBar.setHp(_currentSafeHealth);

        //Win Screen Setup
        winScreen.SetActive(false);

        StartCoroutine(GetSafeHealth());
        StartCoroutine(GetDiff());
        StartCoroutine(getArrest());
    }

    public void DamageSafe()
    {
        if (_currentSafeHealth > 0)
        {
            animator.SetTrigger("ClickTrigger");
            StartCoroutine(DoDamageToSafe());
        }
    }

    private IEnumerator DoDamageToSafe()
    {
        if (!timeOver)
        {
            using var www = new WWW(GameManager.Instance.BASE_URL + "damage_safe" + "/");
            yield return www;
            Debug.Log(www.text);
            var remainingHealth = int.Parse(www.text);
            _currentSafeHealth = remainingHealth;
            hpBar.setHp(_currentSafeHealth);
        }
    }

    private IEnumerator GetSafeHealth()
    {
        while (!gameComplete)
        {
            using var www = new WWW(GameManager.Instance.BASE_URL + "get_safe_hp" + "/");
            yield return www;
            _currentSafeHealth = int.Parse(www.text);
            hpBar.setHp(_currentSafeHealth);
            if (_currentSafeHealth <= 0)
            {
                hpBar.setHp(0);
                gameComplete = true;
                StartCoroutine(SuccessfulRobbery());
                break;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator GetDiff()
    {
        while (!gameComplete)
        {
            using var www = new WWW(GameManager.Instance.BASE_URL + "getTimeUntilEnd" + "/");
            yield return www;
            currentTakenTime = www.text.Replace(",", ":");
            var currentDiff = www.text.Split(":");
            //Debug.Log(www.text);
            var maxDateTime = new DateTime(2000, 1, 1, 12,
                GameManager.Instance.currentMinutes, GameManager.Instance.currentSeconds);
            var diffDateTime = new DateTime(2000, 1, 1, 12,
                int.Parse(currentDiff[1]), int.Parse(currentDiff[2].Split(".")[0]));
            timerText.text = "Time Left: " + maxDateTime.Subtract(diffDateTime).ToString().Split(":")[1] + ":" +
                             maxDateTime.Subtract(diffDateTime).ToString().Split(":")[2];
            if (int.Parse(currentDiff[1]) >= GameManager.Instance.currentMinutes &&
                int.Parse(currentDiff[2].Split(".")[0]) >= GameManager.Instance.currentSeconds)
            {
                timeOver = true;
                gameComplete = true;
                StartCoroutine(FailedRobbery());
                break;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    public IEnumerator getArrest()
    {
        while (!gameComplete)
        {
            using var www = new WWW(GameManager.Instance.BASE_URL + "get_arrest_status/");
            yield return www;
            Debug.Log("arrested: " + www.text);
            string[] subs = www.text.Split("|");
            bool arrested = bool.Parse(subs[0]);
            int penalty = int.Parse(subs[1]);
            if (arrested && penalty == 1)
            {
                StartCoroutine(FailedRobbery());
            }
            else if(arrested && penalty == 0)
            {
                StartCoroutine(FailedRobberyWithout());
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator SuccessfulRobbery()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "end_robbery_success" + "/");
        yield return www;
        var response = S_Parser.ParseResponse(www.text);

        timerTextWinScreen.text = "Time Taken: " + currentTakenTime;
        moneyGain.text = "Money Gained: " + response[1];
        totalMoneyWinScreen.text = "New Balance: " + response[0].Split(".")[0];
        GameManager.Instance.money = int.Parse(response[0].Split(".")[0]);
        winScreen.SetActive(true);
        if (GameManager.Instance.role)
            StartCoroutine(GameManager.Instance.resetRobUnionSafeID());
    }

    private IEnumerator FailedRobbery()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "end_robbery_unsuccess" + "/");
        yield return www;
        Debug.Log(www.text);
        var response = S_Parser.ParseResponse(www.text);

        remainingSafeHP.text = "Remaining Safe Health: " + _currentSafeHealth;
        moneyLose.text = "Money Lost: " + int.Parse(response[1]) / 2;
        totalMoneyLostScreen.text = "New Balance: " + response[0].Split(".")[0];
        GameManager.Instance.money = int.Parse(response[0].Split(".")[0]);
        lostScreen.SetActive(true);
        if (GameManager.Instance.role)
            StartCoroutine(GameManager.Instance.resetRobUnionSafeID());
    }
    
    private IEnumerator FailedRobberyWithout() //TODO ADD TO OTHER MINIGAMES
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "end_robbery_unsuccess_without_penalty" + "/");
        yield return www;
        Debug.Log(www.text);
        
        remainingSafeHP.text = "Remaining Safe Health: " + _currentSafeHealth;
        moneyLose.text = "The cops released you without a fine";
        totalMoneyLostScreen.text = "New Balance: " + www.text;
        lostScreen.SetActive(true);
        if (GameManager.Instance.role)
            StartCoroutine(GameManager.Instance.resetRobUnionSafeID());
    }

    public void CloseButton()
    {
        SceneManager.LoadScene(1);
    }
    
    //todo add damage multiplier x2 effect
}