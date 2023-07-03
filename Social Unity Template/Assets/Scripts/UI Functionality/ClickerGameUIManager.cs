using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickerGameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button safe;
    [SerializeField] private TextMeshProUGUI clickDamageMultiplierText;
    [HideInInspector] public float currentSafeHealth;
    [HideInInspector] public float maxSafeHealth;

    [HideInInspector] public int safeLevel = 1;

    [SerializeField] public Sprite[] safeSprites;
    [SerializeField] private Image currentImage;

    [SerializeField] private Animator animator;

    [SerializeField] public HealthBar hpBar;

    [SerializeField] public GameObject winScreen;

    private float _clickDamageMultiplier;

    private float finalTime;

    private string[] players;

    private float startTime;

    // Start is called before the first frame update
    private void Start()
    {
        _clickDamageMultiplier = 1; //get from GameManager
        currentSafeHealth = GameManager.Instance.currentHP; //get from gameManager according to level
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
    private void Update()
    {
        /*if (hpBar.slider.value == 0)
        {
            if (finalTime == 0)
            {
                finalTime = Time.time - startTime;
                winScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("In " + finalTime.ToString()
                    .Substring(
                        0, 5) + " Seconds");
                var participants = "The Crew: ";
                foreach (var p in players) participants += "\n" + p;
                winScreen.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText(participants);
                var reward = "Your Loot: $$$$$$$";
                winScreen.transform.GetChild(4).GetComponent<TextMeshProUGUI>().SetText(reward);
            }

            winScreen.SetActive(true);
            //Todo: win screen
        }*/
    }

    public void DamageSafe()
    {
        /*if (currentSafeHealth != 0)
        {
            currentSafeHealth -= 1 * _clickDamageMultiplier;
            animator.SetTrigger("ClickTrigger");
            hpBar.setHp(currentSafeHealth);
        }*/
    }

    private IEnumerator DoDamageToSafe()
    {
        using var www = new WWW(GameManager.Instance.client.BASE_URL + "damage_safe" + "/");
        yield return www;
        var dealtDamage = int.Parse(www.text);
        currentSafeHealth -= dealtDamage;
        hpBar.setHp(currentSafeHealth);
    }
}