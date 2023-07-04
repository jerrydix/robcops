using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_PoliceStationController : MonoBehaviour
{
    public GameObject ListPrefab;
    private int money = 0;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI playerMoney;
    public GameObject shop;
    public GameObject membersUi;
    public GameObject panel;
    private int id;
    private string name;
    private int weaponlvl;
    private int armorlvl;
    private int hints;
    private string locationX;
    private string locationY;
    public TextMeshProUGUI weaponlvlText;
    public TextMeshProUGUI armorlvlText;
    public TextMeshProUGUI hintsText;
    public TextMeshProUGUI locationXText;
    public TextMeshProUGUI locationYText;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetMoney());
        StartCoroutine(GetPlayerMoney());
        StartCoroutine(getInfo());
        membersUi.SetActive(true);
        panel = GameObject.FindWithTag("List_Panel");
        deleteChildren();
        membersUi.SetActive(false);
        
        Debug.Log(panel);
    }

    public void deleteChildren()
    {
        foreach (Transform child in panel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public IEnumerator get_members()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "get_station_members/");
        yield return www;
        string[] members = www.text.Split("|");
        for (int i = 0; i < members.Length; i++)
        {
            GameObject element = Instantiate(ListPrefab, panel.transform);
            S_ListElement script = element.GetComponent<S_ListElement>();
            script.SetParameters(i+1 + ".", members[i], id, true, true);
        }
    }

    public IEnumerator GetMoney()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "get_guild_money/");
        yield return www;
        Debug.Log(www.text);
        DisplayMoney(int.Parse(www.text));
    }
    
    public IEnumerator GetPlayerMoney()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "get_money/");
        yield return www;
        Debug.Log(www.text);
        DisplayPlayerMoney(int.Parse(www.text));
    }

    public void DisplayMoney(int money)
    {
        if (money >= 1000000)
        {
            float amount_H  = money / 1000000f;
            float show = (float) Mathf.Round(amount_H * 10f) / 10f;
            moneyText.text = show + "M";
        }
        else if (money >= 1000)
        {
            float amount_H  = money / 1000f;
            float show = (float) Mathf.Round(amount_H * 10f) / 10f;
            moneyText.text = show + "K";
        }
        else
        {
            moneyText.text = "" + money; 
        }
    }
    
    public void DisplayPlayerMoney(int money)
    {
        if (money >= 1000000)
        {
            float amount_H  = money / 1000000f;
            float show = (float) Mathf.Round(amount_H * 10f) / 10f;
            playerMoney.text = show + "M";
        }
        else if (money >= 1000)
        {
            float amount_H  = money / 1000f;
            float show = (float) Mathf.Round(amount_H * 10f) / 10f;
            playerMoney.text = show + "K";
        }
        else
        {
            playerMoney.text = "" + money; 
        }
    }

    public IEnumerator getInfo()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "get_station_info/");
        yield return www;
        string[] info = www.text.Split("|");
        id = int.Parse(info[0]);
        name = info[1];
        weaponlvl = int.Parse(info[2]);
        armorlvl = int.Parse(info[3]);
        hints = int.Parse(info[4]);
    }

    public void donateToGuild()
    {
        StartCoroutine(Donate());
    }

    public void close()
    {
        shop.SetActive(false);
        membersUi.SetActive(false);
        deleteChildren();
    }

    public void openShop()
    {
        shop.SetActive(true);
    }

    public void openMembers()
    {
        membersUi.SetActive(true);
        StartCoroutine(get_members());
    }
    
    public IEnumerator Donate()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "donate_to_guild/");
        yield return www;
        Debug.Log(www.text);
        DisplayMoney(int.Parse(www.text));
        StartCoroutine(GetPlayerMoney());
    }
    
    public IEnumerator UpgradeWeapons()
    {
        WWWForm form = new WWWForm();
        form.AddField("cost", 50000);
        using var www = new WWW(GameManager.Instance.BASE_URL + "upgrade_weapons/", form);
        yield return www;
        Debug.Log(www.text);
        DisplayMoney(int.Parse(www.text));
        updateText();
    }
    
    public IEnumerator UpgradeArmor()
    {
        WWWForm form = new WWWForm();
        form.AddField("cost", 50000);
        using var www = new WWW(GameManager.Instance.BASE_URL + "upgrade_armor/", form);
        yield return www;
        Debug.Log(www.text);
        DisplayMoney(int.Parse(www.text));
        updateText();
    }
    
    public IEnumerator UpgradeHints()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "update_hints/");
        yield return www;
        Debug.Log(www.text);
        updateText();
    }

    public void upgradeWeapon()
    {
        StartCoroutine(UpgradeWeapons());
    }
    
    public void upgradeArmor()
    {
        StartCoroutine(UpgradeArmor());
    }
    
    public void updateHints()
    {
        StartCoroutine(UpgradeHints());
    }
    
    public void updateText()
    {
        weaponlvlText.text = "Weapons Lvl: " + weaponlvl;
        armorlvlText.text = "Armors Lvl: " + armorlvl;
        hintsText.text = "Hints: " + hints;
        locationXText.text = "X: " + locationX;
        locationYText.text = "Y: " + locationY;
    }
}
