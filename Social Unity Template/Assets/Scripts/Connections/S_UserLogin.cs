using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Connections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_UserLogin : MonoBehaviour
{
    // For now only local address

    [HideInInspector] public string BASE_URL = "http://127.0.0.1:8000/";
    [HideInInspector] public string socialTab = "members/";

    [SerializeField] private LoginScreenUIManager manager;
    [SerializeField] private GameObject roleSwitchScreen;
    private int amountOfClicks;
    private float clickPower;
    private Guild guild; //todo fetch guilds from server before login, save them in eg. game manager
    private Vector2 location;
    private int money;
    private string password;
    private bool role;

    private string success;
    private string username;
    private WebSocket ws;

    //
    // Start is called before the first frame update
    private void Start()
    {
        //StartCoroutine(Login("admin", "root"));
    }

    public void login(string username, string password)
    {
        StartCoroutine(Login(username, password));
    }

    public void signup(string username, string password, string passwordRepeat)
    {
        StartCoroutine(Signup(username, password, passwordRepeat));
    }

    public void logout()
    {
        StartCoroutine(Logout());
    } // ReSharper disable Unity.PerformanceAnalysis
    public IEnumerator Login(string username, string password)
    {
        var form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        using var www = new WWW(BASE_URL + socialTab + "login_user", form);
        yield return www;
        Debug.Log(www.text.TrimStart());
        var success = S_Parser.ParseResponse(www.text)[0];
        if (success == "1")
        {
            SetData(S_Parser.ParseResponse(www.text));
            SceneManager.LoadSceneAsync(1);
        }
    }

    public IEnumerator Signup(string username, string password, string passwordRepeat)
    {
        this.username = username;
        this.password = password;

        var form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password1", password);
        form.AddField("password2", passwordRepeat);
        using var www = new WWW(BASE_URL + socialTab + "register_user", form);
        yield return www;
        Debug.Log(www.text.TrimStart());
        success = S_Parser.ParseResponse(www.text)[0];

        if (success == "1")
        {
            manager.registerScreen.SetActive(false);
            roleSwitchScreen.SetActive(true);
        }
    }

    public void RobberButton()
    {
        login(username, password);
    }

    public void CopButton()
    {
        StartCoroutine(SwitchRole());
    }

    private IEnumerator SwitchRole()
    {
        using var www = new WWW(BASE_URL + "switch_role" + "/");
        yield return www;
        Debug.Log("Robber? " + www.text);
        login(username, password);
    }

    public IEnumerator Logout()
    {
        using var www = new WWW(BASE_URL + socialTab + "logout_user");
        yield return www;
        Debug.Log(www.text.TrimStart());
    }

    public void SetData(List<string> list)
    {
        username = list[1];
        money = int.Parse(list[2]);
        amountOfClicks = int.Parse(list[3]);
        clickPower = float.Parse(list[4]);
        location = new Vector2(float.Parse(list[5]), float.Parse(list[6]));
        role = bool.Parse(list[7]);
        //guild = int.Parse(list[7]);
    }
}