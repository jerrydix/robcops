using System.Collections;
using System.Collections.Generic;
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
    private Guild guild; //todo fetch guilds from server before login, save them in game manager
    private int guildID;
    
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
        //SetData(S_Parser.ParseResponse(www.text, ResponseTypes.Login));
        Debug.Log(www.text.TrimStart());
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
        //SetData(S_Parser.ParseResponse(www.text, ResponseTypes.Login));
        Debug.Log(www.text.TrimStart());
    }

    public IEnumerator Logout()
    {
        using WWW www = new WWW(BASE_URL + socialTab + "logout_user");
        yield return www;
    }
    
    public void SetData(ArrayList list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i]);
        }
        username = (string) list[1];
        money = (int) list[2];
        amountOfClicks = (int) list[3];
        clickPower = (float) list[4];
        location = new Vector2((float)list[5], (float)list[6]);
        role = (bool) list[7];
        guildID = (int) list[8];
    }
}
