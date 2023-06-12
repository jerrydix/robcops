using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_UserLogin : MonoBehaviour
{
    // For now only local address
    
    public string BASE_URL = "http://127.0.0.1:8000/";
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Login());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", "admin");
        form.AddField("password", "root");
        using WWW www = new WWW(BASE_URL + "members/login_user", form);
        yield return www;
        Debug.Log(www.text.TrimStart());
    }
}
