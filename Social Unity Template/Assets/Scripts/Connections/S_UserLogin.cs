using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_UserLogin : MonoBehaviour
{
    // For now only local address
    
    [HideInInspector] public string BASE_URL = "http://127.0.0.1:8000/";
    [HideInInspector] public string socialTab = "members/";
    public string username;
    public string money;
    public string role;
    public Vector2 location = new Vector2();
    public Guild guild;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Login("admin", "root"));
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
    }
}
