using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_UserMoney : MonoBehaviour
{
    private S_UserLogin client;

    private void Start()
    {
        client = GetComponent<S_UserLogin>();
    }

    public void editMoney(int money)
    {
        StartCoroutine(EditMoney(money));
    }

    public void getMoney()
    {
        StartCoroutine(GetMoney());
    }
    
    public IEnumerator GetMoney()
    {
        using WWW www = new WWW(client.BASE_URL + "get_money/");
        yield return www;
        Debug.Log(www.text.TrimStart());
    }
    
    public IEnumerator EditMoney(int money)
    {
        WWWForm form = new WWWForm();
        form.AddField("money", money);
        using WWW www = new WWW(client.BASE_URL + "edit_money/", form);
        yield return www;
        Debug.Log(www.text.TrimStart());
    }
}
