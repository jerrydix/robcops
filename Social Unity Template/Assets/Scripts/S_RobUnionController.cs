using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_RobUnionController : MonoBehaviour
{
    public GameObject machinePrefab;
    public GameObject ListPrefab;
    public GameObject plusPrefab;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI playerMoney;
    public GameObject shop;
    public GameObject machinesUi;
    public GameObject membersUi;
    public GameObject panel;
    private readonly List<GameObject> machines = new();
    private int id;
    public int[,] machinesAmount = new int[2, 3];
    private int money = 0;
    private string name;
    private GameObject startPlus;


    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(CreateUnion());
        StartCoroutine(GetMoney());
        StartCoroutine(GetPlayerMoney());
        StartCoroutine(getInfo());
        membersUi.SetActive(true);
        panel = GameObject.FindWithTag("List_Panel");
        deleteChildren();
        membersUi.SetActive(false);

        Debug.Log(panel);
    }

    public void CloseRobUnionScreen()
    {
        SceneManager.LoadScene(1);
    }

    public void deleteChildren()
    {
        foreach (Transform child in panel.transform) Destroy(child.gameObject);
    }

    public IEnumerator get_members()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "get_robunion_members/");
        yield return www;
        var members = www.text.Split("|");
        for (var i = 0; i < members.Length; i++)
        {
            var element = Instantiate(ListPrefab, panel.transform);
            var script = element.GetComponent<S_ListElement>();
            script.SetParameters(i + 1 + ".", members[i], id, true, false);
        }
    }

    public IEnumerator CreateUnion()
    {
        clearList();
        using var www = new WWW(GameManager.Instance.BASE_URL + "get_machines/");
        yield return www;
        Debug.Log(www.text);
        var amount = int.Parse(www.text);
        if (amount > 0)
        {
            startPlus.Destroy();
            SpawnMachines(int.Parse(www.text));
        }
        else
        {
            startPlus = Instantiate(plusPrefab,
                new Vector3(transform.position.x, transform.position.y - 3, transform.position.z), Quaternion.identity);
        }
    }

    public void BuyNewMachine(int cost)
    {
        StartCoroutine(buyNew(cost));
    }

    public IEnumerator buyNew(int cost)
    {
        var form = new WWWForm();
        form.AddField("cost", cost);
        using var www = new WWW(GameManager.Instance.BASE_URL + "buy_new_machine/", form);
        yield return www;
        Debug.Log(www.text);
        StartCoroutine(CreateUnion());
        StartCoroutine(GetPlayerMoney());
        StartCoroutine(GetMoney());
    }

    public IEnumerator GetMoney()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "get_guild_money/");
        yield return www;
        int money = int.Parse(www.text);
        DisplayMoney(money);
        
    }

    public IEnumerator GetPlayerMoney()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "get_money/");
        yield return www;
        Debug.Log(www.text);
        DisplayPlayerMoney(int.Parse(www.text));
        GameManager.Instance.money = money;
    }

    public void SpawnMachines(int amount)
    {
        if (amount > 0 && amount < 7)
        {
            var l = 0;
            for (var i = 0; i < machinesAmount.GetLength(1); i++)
            for (var k = 0; k < machinesAmount.GetLength(0); k++)
            {
                l++;
                if (l > amount)
                {
                    var posPlus = new Vector3(transform.position.x + k * 4, transform.position.y - 3,
                        transform.position.z - i * 5);
                    machines.Add(Instantiate(plusPrefab, posPlus, Quaternion.identity));
                    return;
                }

                var pos = new Vector3(transform.position.x + k * 4, transform.position.y, transform.position.z - i * 5);
                machines.Add(Instantiate(machinePrefab, pos, new Quaternion(0, 180, 0, 0)));
            }
        }
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
        using var www = new WWW(GameManager.Instance.BASE_URL + "get_robunion_info/");
        yield return www;
        var info = www.text.Split("|");
        id = int.Parse(info[0]);
        name = info[1];
    }

    public void clearList()
    {
        foreach (var machine in machines) Destroy(machine);
        machines.Clear();
    }

    public void donateToGuild()
    {
        StartCoroutine(Donate());
    }

    public void close()
    {
        shop.SetActive(false);
        machinesUi.SetActive(false);
        membersUi.SetActive(false);
        deleteChildren();
    }

    public void openShop()
    {
        shop.SetActive(true);
    }

    public void openMachines()
    {
        machinesUi.SetActive(true);
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

    public void buyPowerUps(int item)
    {
        StartCoroutine(buyPowerUp(item));
    }

    public IEnumerator buyPowerUp(int item)
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "buy_powerup/" + item + "/");
        yield return www;
        Debug.Log(www.text);
        DisplayMoney(int.Parse(www.text));
    }
}