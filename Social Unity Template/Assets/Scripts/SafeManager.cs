using System;
using System.Collections;
using System.Collections.Generic;
using Connections;
using UnityEngine;

public class SafeManager : MonoBehaviour
{
    [SerializeField] private Canvas safeUI;

    public int level;
    public int id;
    public int hp; //todo make ranges of hp depending on level
    public double locationX;
    public double locationY;
    public bool createLobby;
    public int lobbyPlayerCount;
    public List<string> lobbyNames;
    private UIManager _uiManager;

    private S_UserLogin client;

    // Start is called before the first frame update
    private void Awake()
    {
        safeUI.worldCamera = Camera.main;
        createLobby = true;
    }

    private void Start()
    {
        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();
    }

    public void SafeClicked()
    {
        Debug.Log("safe clicked");
        //todo add checkSafe method
        StartCoroutine(checkBreakInStatus());
        if (createLobby)
            StartCoroutine(createBreakIn());
        else
            StartCoroutine(joinBreakIn());

        _uiManager.ActivateDialogue(level, locationX, locationY, createLobby, lobbyPlayerCount, lobbyNames);
    }

    private IEnumerator checkBreakInStatus()
    {
        using var www = new WWW(client.BASE_URL + "checkLobby/" + id + "/");
        yield return www;
        createLobby = Convert.ToBoolean(int.Parse(www.text));
        Debug.Log("create lobby: " + createLobby);
    }

    private IEnumerator createBreakIn()
    {
        using var www = new WWW(client.BASE_URL + "create_lobby/" + id + "/");
        yield return www;
        lobbyNames.Add(www.text);
    }

    private IEnumerator joinBreakIn()
    {
        using var www = new WWW(client.BASE_URL + "joinToEvent/" + id + "/");
        yield return www;
        var response = S_Parser.ParseResponse(www.text);
        if (response[0] == "1")
        {
            lobbyPlayerCount = int.Parse(response[1]);
            for (var i = 2; i < response.Count - 1; i++) //todo check if -1 or not
                lobbyNames.Add(response[i]);
        }
    }
}