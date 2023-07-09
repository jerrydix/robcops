using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_Create : MonoBehaviour
{
    public GameObject createUI;
    public TMP_InputField name;

    public void create()
    {
        if (GameManager.Instance.role == false)
        {
            StartCoroutine(createRob());
        }
        else
        {
            StartCoroutine(createPolice());
        }
    }

    public IEnumerator createRob()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name.text);
        using var www = new WWW(GameManager.Instance.BASE_URL + "create_robunion/", form);
        yield return www;
        GameManager.Instance.guild = int.Parse(www.text);
        SceneManager.LoadScene("Scenes/Robunion");
    }
    
    public IEnumerator createPolice()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name.text);
        using var www = new WWW(GameManager.Instance.BASE_URL + "create_police_station/", form);
        yield return www;
        GameManager.Instance.guild = int.Parse(www.text);
        SceneManager.LoadScene("Scenes/PoliceStation");
    }

    public void activateUI()
    {
        createUI.SetActive(true);
    }
    
    public void deactivateUI()
    {
        createUI.SetActive(false);
    }
}
