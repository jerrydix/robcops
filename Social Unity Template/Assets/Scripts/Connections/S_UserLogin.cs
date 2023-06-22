using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Connections;
using UnityEngine;

public class S_UserLogin : MonoBehaviour
{
    // For now only local address
    
    [HideInInspector] public string BASE_URL = "http://127.0.0.1:8000/";
    [HideInInspector] public string socialTab = "members/";
    private string username;
    private int money;
    private bool role;
    private int amountOfClicks;
    private float clickPower;
    private Vector2 location = new Vector2();
    private Guild guild; //todo fetch guilds from server before login, save them in eg. game manager
    private WebSocket ws;

    [SerializeField] private LoginScreenUIManager manager;
    
    // Start is called before the first frame update
    void Start()
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
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        using WWW www = new WWW(BASE_URL + socialTab + "login_user", form);
        yield return www;
        Debug.Log(www.text.TrimStart());
        SetData(S_Parser.ParseResponse(www.text, ResponseTypes.Login));
    }

    public IEnumerator Signup(string username, string password, string passwordRepeat)
    {
        Debug.Log(username + password + passwordRepeat);
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password1", password);
        form.AddField("password2", passwordRepeat);
        using WWW www = new WWW(BASE_URL + socialTab + "register_user", form);
        yield return www;
        Debug.Log(www.text.TrimStart());
    }

    public IEnumerator Logout()
    {
        using WWW www = new WWW(BASE_URL + socialTab + "logout_user");
        yield return www;
    }
    
    public void SetData(List<string> list)
    {
        username = list[0];
        money = int.Parse(list[1]);
        amountOfClicks = int.Parse(list[2]);
        clickPower = float.Parse(list[3]);
        location = new Vector2(float.Parse(list[4]), float.Parse(list[5]));
        role = bool.Parse(list[6]);
        //guild = int.Parse(list[7]);
    }
}
