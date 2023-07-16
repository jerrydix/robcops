using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Connections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_UserLogin : MonoBehaviour
{
    // For now only local address

    public string BASE_URL = "http://robcops.de/";
    [HideInInspector] public string socialTab = "members/";

    [SerializeField] private LoginScreenUIManager manager;
    [SerializeField] private GameObject roleSwitchScreen;
    [SerializeField] private GameObject logo;
    private int amountOfClicks;
    private float clickPower;
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
        Debug.Log(BASE_URL + socialTab + "login_user");
        yield return www;
        Debug.Log(www.text.TrimStart());
        var success = S_Parser.ParseResponse(www.text)[0];
        if (success == "1")
        {
            GameManager.SetData(S_Parser.ParseResponse(www.text));
            SceneManager.LoadSceneAsync(1);
        }
        else
        {
            GameManager.Instance.errorMessage.PopUp(www.text.Split("|")[1]);
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
            logo.SetActive(false);
            roleSwitchScreen.SetActive(true);
        }
        else
        {
            GameManager.Instance.errorMessage.PopUp(www.text.Split("|")[1]);
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
        Debug.Log("Cop? " + www.text);
        login(username, password);
    }

    public IEnumerator Logout()
    {
        using var www = new WWW(BASE_URL + socialTab + "logout_user");
        yield return www;
        Debug.Log(www.text.TrimStart());
        SceneManager.LoadScene(0);
    }
}