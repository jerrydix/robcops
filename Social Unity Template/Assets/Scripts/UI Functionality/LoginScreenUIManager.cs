using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScreenUIManager : MonoBehaviour
{
    [SerializeField] private GameObject loginScreen;
    [SerializeField] private GameObject registerScreen;
    [SerializeField]

    private void Awake()
    {
        loginScreen.SetActive(true);
        registerScreen.SetActive(false);
    }

    public void LoginButton(bool success)
    {
        if (success)
            SceneManager.LoadScene(1); //todo load map scene
    }

    public void SignupAndLogin(bool success) //todo make coupled
    {
        if (success)
        {
            //todo automatically log in
            
        }
    }

    public void BackButton()
    {
        registerScreen.SetActive(false);
        loginScreen.SetActive(true);
    }

    public void NewPlayerButton()
    {
        loginScreen.SetActive(false);
        registerScreen.SetActive(true);
    }
    
    
}
