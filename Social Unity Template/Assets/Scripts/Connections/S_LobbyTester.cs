using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_LobbyTester : MonoBehaviour
{
    private S_UserLogin client;
    public TMP_InputField id;

    private void Start()
    {
        client = GameObject.FindWithTag("Server").GetComponent<S_UserLogin>();
        Debug.Log(client);
    }
    // Start is called before the first frame update
    public void createSafe()
    {
        StartCoroutine(CreateSafe());
    }

    public void createLobby()
    {
        int safe_id = int.Parse(id.text);
        StartCoroutine(CreateLobby(safe_id));
    }
    
    public IEnumerator CreateSafe()
    {
        using WWW www = new WWW(client.BASE_URL + "create_safe/");
        yield return www;
        Debug.Log(www.text);
    }

    public IEnumerator CreateLobby(int safe_id)
    {
        using WWW www = new WWW(client.BASE_URL + "create_lobby/" + safe_id + "/");
        yield return www;
        Debug.Log(www.text);
    }
    
}
