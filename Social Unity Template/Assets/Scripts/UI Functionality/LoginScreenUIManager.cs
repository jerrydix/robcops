using UnityEngine;

public class LoginScreenUIManager : MonoBehaviour
{
    [SerializeField] private GameObject loginScreen;
    [SerializeField] public GameObject registerScreen;

    private void Awake()
    {
        loginScreen.SetActive(true);
        registerScreen.SetActive(false);
    }

    public void BackButton()
    {
        registerScreen.SetActive(false);
        loginScreen.SetActive(true);
    }

    public void NewPlayerButton()
    {
        loginScreen.SetActive(false);
        registerScreen.SetActive(true);
    }
}