using System.Collections;
using TMPro;
using UnityEngine;

public class S_PersonalUpgrades : MonoBehaviour
{
    public TextMeshProUGUI amountLevel;
    public TextMeshProUGUI powerLevel;

    private void Start()
    {
        StartCoroutine(getAmount());
        StartCoroutine(getPower());
    }

    public IEnumerator getAmount()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "get_amount_of_clicks/");
        yield return www;
        amountLevel.text = "Lvl: " + www.text;
       
    }

    public IEnumerator getPower()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "get_click_power/");
        yield return www;
        powerLevel.text = "Lvl: " + www.text;
       
    }

    public void UpgradeAmount()
    {
        StartCoroutine(upgradeAmount());
    }

    public void UpgradePower()
    {
        StartCoroutine(upgradePower());
    }

    public IEnumerator upgradeAmount()
    {
        var form = new WWWForm();
        form.AddField("cost", 50000);
        using var www = new WWW(GameManager.Instance.BASE_URL + "upgrade_amount_of_clicks/", form);
        yield return www;
        var subs = www.text.Split("|");
        if (int.Parse(subs[0]) == 0)
        {
            Debug.Log("Not enough Money");
            GameManager.Instance.errorMessage.PopUp(www.text.Split("|")[1]);
        }
        else
        {
            GameManager.Instance.successMessage.PopUp("Base damage increased!");
            amountLevel.text = "Lvl: " + subs[1];
            StartCoroutine(GameManager.Instance.GetPlayerMoneyOnce());
        }
    }

    public IEnumerator upgradePower()
    {
        var form = new WWWForm();
        form.AddField("cost", 50000);
        using var www = new WWW(GameManager.Instance.BASE_URL + "upgrade_click_power/", form);
        yield return www;
        var subs = www.text.Split("|");
        if (int.Parse(subs[0]) == 0)
        {
            GameManager.Instance.errorMessage.PopUp(www.text.Split("|")[1]);
            Debug.Log("Not enough Money");
        }
        else
        {
            GameManager.Instance.successMessage.PopUp("Damage multiplied!");
            
            powerLevel.text = "Lvl: " + int.Parse(subs[1]);
            StartCoroutine(GameManager.Instance.GetPlayerMoneyOnce());
        }
    }
}