using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class F_SignupForm : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password1;
    public TMP_InputField password2;
    private S_UserLogin signupScript;

    private void Start()
    {
        signupScript = GameObject.FindWithTag("Server").GetComponent<S_UserLogin>();
    }

    public void SendInfo()
    {
        if (password1.text.Equals(password2.text))
        {
            signupScript.signup(username.text, password1.text, password2.text);
        }
        else
        {
            Debug.Log("Password are not same");
        }
    }
}
