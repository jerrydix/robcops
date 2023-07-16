using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_PoliceStationController : MonoBehaviour
{
    public GameObject ListPrefab;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI playerMoney;
    public GameObject shop;
    public GameObject membersUi;
    public GameObject panel;
    public TextMeshProUGUI weaponlvlText;
    public TextMeshProUGUI armorlvlText;
    public TextMeshProUGUI hintsText;
    public TextMeshProUGUI locationXText;
    public TextMeshProUGUI locationYText;
    private int armorlvl;
    private int hints;
    private int id;
    private string locationX;
    private string locationY;
    private int money = 0;
    private string name;
    private int weaponlvl;


    // Start is called before the first frame update
    private void Start()
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

    public void leaveGuild()
    {
        StartCoroutine(Leave());
    }

    public IEnumerator Leave()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "leave_guild/");
        yield return www;
        GameManager.Instance.guild = -1;
        SceneManager.LoadScene(1);
    }

    public void deleteChildren()
    {
        foreach (Transform child in panel.transform) Destroy(child.gameObject);
    }

    public IEnumerator get_members()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "get_station_members/");
        yield return www;
        var members = www.text.Split("|");
        for (var i = 0; i < members.Length; i++)
        {
            var element = Instantiate(ListPrefab, panel.transform);
            var script = element.GetComponent<S_ListElement>();
            script.SetParameters(i + 1 + ".", members[i], id, true, true);
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
        int money = int.Parse(www.text);
        DisplayPlayerMoney(money);
        GameManager.Instance.money = money;
    }

    public void DisplayMoney(int money)
    {
        if (money >= 1000000)
        {
            var amount_H = money / 1000000f;
            var show = Mathf.Round(amount_H * 10f) / 10f;
            moneyText.text = show + "M";
        }
        else if (money >= 1000)
        {
            var amount_H = money / 1000f;
            var show = Mathf.Round(amount_H * 10f) / 10f;
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
            var amount_H = money / 1000000f;
            var show = Mathf.Round(amount_H * 10f) / 10f;
            playerMoney.text = show + "M";
        }
        else if (money >= 1000)
        {
            var amount_H = money / 1000f;
            var show = Mathf.Round(amount_H * 10f) / 10f;
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
        var info = www.text.Split("|");
        id = int.Parse(info[0]);
        name = info[1];
        weaponlvl = int.Parse(info[2]);
        armorlvl = int.Parse(info[3]);
        hints = int.Parse(info[4]);
        locationX = info[5];
        locationY = info[6];
        updateText();
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
        var form = new WWWForm();
        form.AddField("cost", 100000);
        using var www = new WWW(GameManager.Instance.BASE_URL + "upgrade_weapons/", form);
        yield return www;
        int result = -1;
        if (int.TryParse(www.text, out result))
        {
            DisplayMoney(result);
            GameManager.Instance.successMessage.PopUp("You just upgraded your weapons for your squad!");
        }
        else

        {
            GameManager.Instance.errorMessage.PopUp(
                "The police station doesn't have enough money to buy more weapons!");
        }

        Debug.Log(www.text);
        StartCoroutine(getInfo());
    }

    public IEnumerator UpgradeArmor()
    {
        var form = new WWWForm();
        form.AddField("cost", 100000);
        using var www = new WWW(GameManager.Instance.BASE_URL + "upgrade_armor/", form);
        yield return www;
        int result = -1;
        if (int.TryParse(www.text, out result))
        {
            DisplayMoney(result);
            GameManager.Instance.successMessage.PopUp("You just upgraded your armor for your squad!");
        }
        else
        {
            GameManager.Instance.errorMessage.PopUp("The police station doesn't have enough money to buy more armor!");
        }

        Debug.Log(www.text);
        StartCoroutine(getInfo());
    }

    public IEnumerator UpgradeHints()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "update_hints/");
        yield return www;
        Debug.Log(www.text);
        StartCoroutine(getInfo());
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
        hintsText.text = "Hints: " + hints + "/5";
        locationXText.text = "LON:\n" + locationX;
        locationYText.text = "LAT:\n" + locationY;
    }

    public void ClosePoliceStationScreen()
    {
        SceneManager.LoadScene(1);
    }
}