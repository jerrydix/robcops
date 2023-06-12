using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginMenu : MonoBehaviour
{
  public TMP_InputField nameInput;
  public TMP_InputField passwordInput;
  public Button loginButton;

  public int nameLengthMin = 4;
  public int passwordLengthMin = 8;
  public string loginUrl = "login/";

  public void CallLogin()
  {
    StartCoroutine(Login());
  }

  IEnumerator Login()
  {
    WWWForm form = new WWWForm();
    form.AddField("username", nameInput.text);
    form.AddField("password", passwordInput.text);
    using WWW www = new WWW(Client.BASE_URL + loginUrl, form);
    yield return www;
    string wwwText = www.text.TrimStart();
    Debug.Log(wwwText);
    if (wwwText.StartsWith("0") || wwwText.StartsWith("1"))
    {
      SceneManager.LoadScene("MainMenu");
    }
  }

  public void VerifyInputs()
  {
    bool nameVerified = nameInput.text.Length >= nameLengthMin;
    bool passwordVerified = passwordInput.text.Length >= passwordLengthMin;
    loginButton.interactable = nameVerified && passwordVerified && nameInput.text != passwordInput.text;
  }

}
