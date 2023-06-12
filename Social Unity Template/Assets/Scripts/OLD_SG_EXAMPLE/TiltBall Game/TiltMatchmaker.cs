
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TiltMatchmaker : MonoBehaviour
{
  public string hostMatchUrl = "host_match/";
  public string joinMatchUrl = "join_match/";
  public TMP_Text infoField;
  public TMP_InputField nameInput;
  public Button joinButton;
  public Button hostButton;
  public Button exitButton;
  TiltGame tiltGame;

  // Start is called before the first frame update
  void Start()
  {
    tiltGame = gameObject.GetComponent<TiltGame>();
  }

  IEnumerator JoinMatch()
  {
    string host = nameInput.text;
    WWWForm form = new WWWForm();
    form.AddField("host", host);
    using (WWW www = new WWW(Client.BASE_URL + joinMatchUrl, form))
    {
      yield return www;
      string wwwText = www.text.TrimStart();
      Debug.Log(wwwText);
      if (wwwText.StartsWith("0"))
      {
        tiltGame.host = host;
        tiltGame.isHost = false;
        infoField.text = host + "'s Match";
        nameInput.gameObject.SetActive(false);
        joinButton.gameObject.SetActive(false);
        hostButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
      }
    }
  }

  IEnumerator HostMatch()
  {
    using (WWW www = new WWW(Client.BASE_URL + hostMatchUrl))
    {
      yield return www;
      string wwwText = www.text.TrimStart();
      Debug.Log(wwwText);
      if (wwwText.StartsWith("0"))
      {
        tiltGame.host = Client.username;
        tiltGame.isHost = true;
        infoField.text = "Your Match";
        nameInput.gameObject.SetActive(false);
        joinButton.gameObject.SetActive(false);
        hostButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
      }
    }
  }

  public void CallJoinMatch()
  {
    StartCoroutine(JoinMatch());
  }
  public void CallHostMatch()
  {
    StartCoroutine(HostMatch());
  }

  public void LoadMainMenu()
  {
    SceneManager.LoadScene("MainMenu");
  }

}
