using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SafeUIManager : MonoBehaviour
{
    // Start is cal] led before the first frame update
    [SerializeField] private TextMeshProUGUI safeLevelTextDialogue;
    [SerializeField] private TextMeshProUGUI locationTextDialogue;
    [SerializeField] private TextMeshProUGUI safeLevelTextScreen;
    [SerializeField] private TextMeshProUGUI locationTextScreen;
    [SerializeField] private TextMeshProUGUI membersText;
    [SerializeField] private TextMeshProUGUI lobbyCountText;
    [SerializeField] private int wireCutterCountText;
    [SerializeField] private int c4CountText;


    [SerializeField] private GameObject heistPrepScreen;

    [SerializeField] private GameObject safeDialogue;
    //todo variables for UI changes, like when user presses button to buy c4, wire cutters, personal power ups etc.

    [HideInInspector] public string[] members; //todo get members from game manager / server

    public int minigameIndex; //todo get minigame index from server
    private int _c4Count;

    private bool _createLobby;
    private List<string> _lobbyNames;
    private int _lobbyPlayerCount;
    private int _wireCutterCount;

    //todo animation with dots changing for waiting for players

    public void InitializeSafe(int level, double locationX, double locationY, bool createLobby, int playerCount,
        List<string> names)
    {
        _createLobby = createLobby;
        _lobbyPlayerCount = playerCount;
        _lobbyNames = names;
        safeLevelTextDialogue.text = safeLevelTextScreen.text = "Level " + level + " Safe";
        locationTextDialogue.text = locationTextScreen.text = "Coordinates: " +
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
        //todo add either join or 


        heistPrepScreen.SetActive(true);
        safeDialogue.SetActive(false);
    }

    public void PressCancelButton()
    {
        safeDialogue.SetActive(false);
    }

    public void RobberyButton()
    {
        SceneManager
            .LoadSceneAsync(
                minigameIndex); //todo add mini game parameters thru loading them in gamemanager first, then taking them from there
    }

    public void CancelButton()
    {
        //todo inform server that user has left the lobby
        safeDialogue.SetActive(false);
    }

    public void WireCutterButton()
    {
        _wireCutterCount++;
    }

    public void C4Button()
    {
        _c4Count++;
    }

    //TODO Powerup shop + guild powerup UI add, remove
}