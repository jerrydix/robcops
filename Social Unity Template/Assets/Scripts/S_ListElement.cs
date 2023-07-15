using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_ListElement : MonoBehaviour
{
    public TextMeshProUGUI number;
    public TextMeshProUGUI name;
    public Button joinButton;
    public bool list;
    public int guildId;
    public bool cop;

    // Start is called before the first frame update
    private void Start()
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
                joinButton.onClick.AddListener(set_cop);
            else
                joinButton.onClick.AddListener(set_rob);
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
        using var www = new WWW(GameManager.Instance.BASE_URL + "join_rob_union/" + guildId + "/");
        yield return www;
        Debug.Log(www.text);
        if (www.text.Split("|")[0].Equals("0"))
        {
            GameManager.Instance.errorMessage.PopUp(www.text.Split("|")[1]);
        }
        GameManager.Instance.guild = guildId;
        SceneManager.LoadScene("Scenes/Robunion");
    }

    public IEnumerator join_cop()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "join_police_station/" + guildId + "/");
        yield return www;
        if (www.text.Split("|")[0].Equals("0"))
        {
            GameManager.Instance.errorMessage.PopUp(www.text.Split("|")[1]);
        }
        Debug.Log(www.text);
        GameManager.Instance.guild = guildId;
        SceneManager.LoadScene("Scenes/PoliceStation");
    }
}