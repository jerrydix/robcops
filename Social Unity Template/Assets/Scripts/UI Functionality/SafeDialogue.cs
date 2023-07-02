using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SafeDialogue : MonoBehaviour
{
    // Start is cal] led before the first frame update
    [SerializeField] private TextMeshProUGUI safeLevelText;
    [SerializeField] private TextMeshProUGUI locationText;

    [SerializeField] private GameObject heistPrepScreen;
    //todo variables for UI changes, like when user presses button to buy c4, wire cutters, personal power ups etc.

    [HideInInspector] public string[] members; //todo get members from game manager / server

    public int minigameIndex; //todo get minigame index from server
    private int _c4Count;
    private int _wireCutterCount;

    //todo animation with dots changing for waiting for players


    public void InitializeSafe(int level, double locationX, double locationY)
    {
        safeLevelText.text = "Level " + level + " Safe";
        locationText.text = "Coordinates: " +
                            Math.Round(locationX, 4).ToString(CultureInfo.CurrentCulture).Replace(",", ".") + ", " +
                            Math.Round(locationY, 4).ToString(CultureInfo.CurrentCulture).Replace(",", ".");
    }

    public void LobbyScreenButton()
    {
        heistPrepScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    public void PressCancelButton()
    {
        gameObject.SetActive(false);
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
        gameObject.SetActive(false);
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