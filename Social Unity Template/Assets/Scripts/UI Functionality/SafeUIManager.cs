using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Connections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class SafeUIManager : MonoBehaviour
{
    // Start is cal] led before the first frame update
    [SerializeField] private TextMeshProUGUI safeLevelTextDialogue;
    [SerializeField] private TextMeshProUGUI locationTextDialogue;
    [SerializeField] private TextMeshProUGUI safeLevelTextScreen;
    [SerializeField] private TextMeshProUGUI membersText;
    [SerializeField] private TextMeshProUGUI lobbyCountText;
    [SerializeField] private TextMeshProUGUI wireCutterCountText;
    [SerializeField] private TextMeshProUGUI c4CountText;


    [SerializeField] private GameObject heistPrepScreen;

    [SerializeField] private GameObject safeDialogue;
    //todo variables for UI changes, like when user presses button to buy c4, wire cutters, personal power ups etc.

    [HideInInspector] public string[] members; //todo get members from game manager / server

    private bool _createLobby;
    private int _id;
    private List<string> _lobbyNames;
    private int _lobbyPlayerCount;
    private bool isRURaid;

    //todo animation with dots changing for waiting for players

    private void Start()
    {
        isRURaid = false;
    }

    public void InitializeSafe(int level, double locationX, double locationY, bool createLobby, int id)
    {
        _createLobby = createLobby;
        _id = id;
        safeLevelTextDialogue.text = "Level " + level + " Safe";
        safeLevelTextScreen.text = "Level " + level + " Safe\nWaiting for Players...";
        locationTextDialogue.text = "Coordinates: " +
                                    Math.Round(locationX, 4)
                                        .ToString(CultureInfo.CurrentCulture)
                                        .Replace(",", ".") +
                                    ", " +
                                    Math.Round(locationY, 4)
                                        .ToString(CultureInfo.CurrentCulture)
                                        .Replace(",", ".");
    }
    
    public void InitializeRURaidSafe(int level, double locationX, double locationY, bool createLobby, int id)
    {
        isRURaid = true;
        _createLobby = createLobby;
        _id = id;
        safeLevelTextDialogue.text = "Level " + level + " RobUnion";
        safeLevelTextScreen.text = "Level " + level + " RobUnion\nWaiting for Players...";
        locationTextDialogue.text = "Coordinates: " +
                                    Math.Round(locationX, 4)
                                        .ToString(CultureInfo.CurrentCulture)
                                        .Replace(",", ".") +
                                    ", " +
                                    Math.Round(locationY, 4)
                                        .ToString(CultureInfo.CurrentCulture)
                                        .Replace(",", ".");
    }

    public void LobbyScreenButton()
    {
        if (_createLobby)
            StartCoroutine(CreateBreakIn());
        else
            StartCoroutine(JoinBreakIn());
    }

    public void RobberyButton()
    {
        StartCoroutine(StartRobbery());
    }

    public void CancelButton()
    {
        if (_createLobby)
            StartCoroutine(DisbandLobby());
        else
            StartCoroutine(LeaveLobby());
    }

    public void WireCutterButton()
    {
        StartCoroutine(AddWireCutter());
    }

    private IEnumerator AddWireCutter()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "add_upgrades_to_lobby/" + 1 + "/");
        yield return www;
        Debug.Log(www.text);
        if (S_Parser.ParseResponse(www.text)[0] == "0")
            yield break;
        wireCutterCountText.text = www.text;
    }

    public void C4Button()
    {
        StartCoroutine(AddC4());
    }

    private IEnumerator AddC4()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "add_upgrades_to_lobby/" + 0 + "/");
        yield return www;
        Debug.Log(www.text);
        if (S_Parser.ParseResponse(www.text)[0] == "0")
            yield break;
        c4CountText.text = www.text;
    }

    private IEnumerator CreateBreakIn()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "create_lobby/" + _id + "/");
        yield return www;
        _lobbyPlayerCount = 1;
        Debug.Log(_lobbyPlayerCount);
        _lobbyNames = new List<string>();
        _lobbyNames.Add(www.text);
        membersText.text = _lobbyNames[0];
        _lobbyPlayerCount = 1;
        lobbyCountText.text = _lobbyPlayerCount + "/5";
        heistPrepScreen.SetActive(true);
        safeDialogue.SetActive(false);
        StartCoroutine(UpdateSafeUI());
    }

    private IEnumerator JoinBreakIn()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "joinToEvent/" + _id + "/");
        yield return www;
        _lobbyNames = new List<string>();
        var response = S_Parser.ParseResponse(www.text);
        if (response[0] == "1")
        {
            _lobbyPlayerCount = int.Parse(response[1]);
            for (var i = 2; i < response.Count - 1; i++) //todo check if -1 or not
                _lobbyNames.Add(response[i]);
        }
        else
        {
            Debug.Log("lobby full / started");
        }

        var text = "";
        foreach (var member in _lobbyNames) text += member + "\n";
        membersText.text = text;
        lobbyCountText.text = _lobbyPlayerCount + "/5";
        heistPrepScreen.SetActive(true);
        safeDialogue.SetActive(false);
        StartCoroutine(UpdateSafeUI());
    }

    private IEnumerator DisbandLobby()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "destroy_event" + "/");
        yield return www;
        safeDialogue.SetActive(true);
        heistPrepScreen.SetActive(false);
        gameObject.SetActive(false);
    }

    private IEnumerator LeaveLobby()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "leave_lobby" + "/");
        yield return www;
        safeDialogue.SetActive(true);
        heistPrepScreen.SetActive(false);
        gameObject.SetActive(false);
    }

    private IEnumerator UpdateSafeUI()
    {
        while (true)
        {
            using var www = new WWW(GameManager.Instance.BASE_URL + "check_lobby_info" + "/");
            yield return www;
            var response = S_Parser.ParseResponse(www.text);
            Debug.Log(www.text);
            if (Convert.ToBoolean(response[1]))
            {
                RobberyButton();
                break;
            }

            lobbyCountText.text = response[0] + "/5";
            c4CountText.text = response[2];
            wireCutterCountText.text = response[3];
            var currentMembersText = "";
            for (var i = 4; i < response.Count - 1; i++) currentMembersText += response[i] + "\n";
            membersText.text = currentMembersText;
            Debug.Log(lobbyCountText.text + " " + c4CountText.text + " " + wireCutterCountText.text + " " +
                      membersText.text);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator StartRobbery()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "start_robbery" + "/");
        yield return www;
        Debug.Log(www.text);
        var response = S_Parser.ParseResponse(www.text);
        Debug.Log("time: " + response[0]);
        Debug.Log("hp: " + response[1]);

        GameManager.Instance.currentHP = int.Parse(response[1]);
        GameManager.Instance.currentMinutes = int.Parse(response[0].Split(".")[0]);
        GameManager.Instance.currentSeconds = int.Parse(response[0].Split(".")[1]);
        GameManager.Instance.currentSafeLevel = int.Parse(response[2]);

        var rnd = new Random();
        var index = rnd.Next(1, 4);
        switch (index)
        {
            case 1:
                SceneManager.LoadScene(isRURaid ? "Scenes/ClickerGameRURaid" : "Scenes/ClickerGame");
                break;
            case 2:
                SceneManager.LoadScene(isRURaid ? "Scenes/MemoryGameRURaid" : "Scenes/MemoryGame");
                break;
            case 3:
                SceneManager.LoadScene(isRURaid ? "Scenes/RotationGameRURaid" : "Scenes/RotationGame");
                break;
        }
    }
    
}