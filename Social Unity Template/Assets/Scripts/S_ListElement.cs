using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class S_ListElement : MonoBehaviour
{
    public TextMeshProUGUI number;
    public TextMeshProUGUI name;
    public Button joinButton;
    public bool list;
    public int guildId = 0;
    public bool cop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetParameters(string num, string nennung, int id, bool isList, bool isCop)
    {
        number.text = num;
        name.text = nennung;
        guildId = id;
        list = isList;
        cop = isCop;
        if (list)
        {
            joinButton.gameObject.SetActive(false);
        }
        else
        {
            if (cop)
            {
                joinButton.onClick.AddListener(set_cop);
            }
            else
            {
                joinButton.onClick.AddListener(set_rob);
            }
        }
    }

    public void set_rob()
    {
        StartCoroutine(join_rob());
    }
    
    public void set_cop()
    {
        StartCoroutine(join_cop());
    }
    
    public IEnumerator join_rob()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "join_rob_union/" + guildId.ToString() + "/");
        yield return www;
        Debug.Log(www.text);
    }
    
    public IEnumerator join_cop()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "join_police_station/" + guildId.ToString() + "/");
        yield return www;
        Debug.Log(www.text);
    }
}
