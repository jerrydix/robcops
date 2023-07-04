using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_RobUnionController : MonoBehaviour
{
    public int[,] machinesAmount = new int[2,3];
    public GameObject machinePrefab;
    public GameObject plusPrefab;
    private List<GameObject> machines = new List<GameObject>();
    private int money = 0;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI playerMoney;

    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CreateUnion());
        StartCoroutine(GetMoney());
        StartCoroutine(GetPlayerMoney());
        //StartCoroutine(getInfo());
    }

    public IEnumerator CreateUnion()
    {
        clearList();
        using var www = new WWW(GameManager.Instance.BASE_URL + "get_machines/");
        yield return www;
        Debug.Log(www.text);
        int amount = int.Parse(www.text);
        if (amount > 0)
        {
            SpawnMachines(int.Parse(www.text));
        }
        else
        {
            Instantiate(plusPrefab, new Vector3(transform.position.x, transform.position.y - 3, transform.position.z), Quaternion.identity);
        }
    }

    public IEnumerator buyNew(int cost)
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "buy_new_machine/" + cost + "/");
        yield return www;
        Debug.Log(www.text);
        StartCoroutine(CreateUnion());
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

    public void SpawnMachines(int amount)
    {
        if (amount > 0 && amount < 7)
        {
            int l = 0;
            for (int i = 0; i < machinesAmount.GetLength(1); i++)
            {
                for (int k = 0; k < machinesAmount.GetLength(0); k++)
                {
                    l++;
                    if (l > amount)
                    {
                        Vector3 posPlus = new Vector3(transform.position.x + k*4, transform.position.y - 3, transform.position.z - i*5);
                        machines.Add(Instantiate(plusPrefab, posPlus, Quaternion.identity));
                        return;
                    }
                    Vector3 pos = new Vector3(transform.position.x + k*4, transform.position.y, transform.position.z - i*5);
                    machines.Add(Instantiate(machinePrefab, pos, new Quaternion(0, 180, 0, 0)));
                }
            }
        }
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
        yield return new WaitForSeconds(2);
        clearList();
    }

    public void clearList()
    {
        foreach (GameObject machine in machines)
        {
            Destroy(machine);
        }
        machines.Clear();
    }

    public void donateToGuild()
    {
        StartCoroutine(Donate());
    }
    
    public IEnumerator Donate()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "donate_to_guild/");
        yield return www;
        Debug.Log(www.text);
        DisplayMoney(int.Parse(www.text));
        
    }
}
