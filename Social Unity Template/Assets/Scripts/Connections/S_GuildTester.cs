using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_GuildTester : MonoBehaviour
{
    private S_UserLogin client;
    public TMP_InputField robName;

    private void Start()
    {
        client = GameObject.FindWithTag("Server").GetComponent<S_UserLogin>();
        Debug.Log(client);
    }

    public void createUnion()
    {
        string unionName = robName.text;
        StartCoroutine(CreateUnion(unionName));
    }

    public IEnumerator CreateUnion(string unionName)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", unionName);
        using WWW www = new WWW(client.BASE_URL + "create_test_robunion/", form);
        yield return www;
        Debug.Log(www.text);
    }
}
