using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class F_LoginForm : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password;
    private S_UserLogin loginScript;

    private void Start()
    {
        loginScript = GameObject.FindWithTag("Server").GetComponent<S_UserLogin>();
    }

    public void SendInfo()
    {
        loginScript.login(username.text, password.text);
    }
}
