using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

  public TMP_Text infoField;
  public Button playButton;
  public Button loginButton;
  public Button signupButton;

  [TextArea(1, 4)]
  public string loggedOutInfoText = "Please log in or sign\nup for an account";

  [TextArea(1, 4)]
  public string loggedInInfoText = "Welcome,\n{0}!";

  public string logoutUrl = "logout/";
  public string checkAuthUrl = "check_auth/";

  public void Start()
  {
    StartCoroutine(CheckAuth());
  }

  IEnumerator CheckAuth()
  {
    using (WWW www = new WWW(Client.BASE_URL + checkAuthUrl))
    {
      yield return www;
      string wwwText = www.text.TrimStart();
      Debug.Log(wwwText);
      if (wwwText.StartsWith("0"))
      {
        Client.username = wwwText.Split("\"")[1];
        SwitchToLoggedInView();
      }
      else if (wwwText.StartsWith("1"))
      {
        SwitchToLoggedOutView();
      }
    }
  }

  void SwitchToLoggedInView()
  {
    playButton.interactable = true;
    loginButton.gameObject.SetActive(false);
    signupButton.gameObject.SetActive(false);
    infoField.text = string.Format(loggedInInfoText, Client.username);
  }

  void SwitchToLoggedOutView()
  {
    playButton.interactable = false;
    loginButton.gameObject.SetActive(true);
    signupButton.gameObject.SetActive(true);
    infoField.text = loggedOutInfoText;
  }

  public void CallLogout()
  {
    StartCoroutine(Logout());
  }

  IEnumerator Logout()
  {
    using (WWW www = new WWW(Client.BASE_URL + logoutUrl))
    {
      yield return www;
      string wwwText = www.text.TrimStart();
      Debug.Log(wwwText);
      if (wwwText.StartsWith("0"))
      {
        SwitchToLoggedOutView();
      }
    }
  }

  public void LoadPlayMenu()
  {
    SceneManager.LoadScene("PlayMenu");
  }

  public void LoadLoginMenu()
  {
    SceneManager.LoadScene("LoginMenu");
  }

  public void LoadSignupMenu()
  {
    SceneManager.LoadScene("SignupMenu");
  }
}
