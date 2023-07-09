using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_Create : MonoBehaviour
{
    public GameObject createUI;
    public TMP_InputField name;

    private void Start()
    {
        throw new NotImplementedException();
    }

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
    }
    
    public IEnumerator createPolice()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name.text);
        using var www = new WWW(GameManager.Instance.BASE_URL + "create_police_station/", form);
        yield return www;
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
