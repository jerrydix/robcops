using System;
using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("SAFE ID: " + id);
        if (!GameManager.Instance.role)
            StartCoroutine(checkBreakInStatus());
        //todo arrest when playing cop
    }

    private IEnumerator checkBreakInStatus()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "checkLobby/" + id + "/");
        yield return www;
        Debug.Log(GameManager.Instance.BASE_URL + "checkLobby/" + id + "/");
        createLobby = Convert.ToBoolean(int.Parse(www.text));
        _uiManager.ActivateDialogue(level, locationX, locationY, createLobby, id);
    }
}