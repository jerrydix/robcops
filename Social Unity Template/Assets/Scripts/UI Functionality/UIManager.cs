using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private SafeDialogue dialogue;
    [SerializeField] private GameObject mapScreenUI;
    [SerializeField] private GameObject guildUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject settingsUI;

    public void ActivateDialogue(string safeDescriptionText, string addressText, string lobbyText)
    {
        dialogue.gameObject.SetActive(true);
        dialogue.InitializeSafe(safeDescriptionText, addressText, lobbyText);
    }

    public void GuildButton()
    {
        mapScreenUI.SetActive(false);
        guildUI.SetActive(true);
    }

    public void ShopButton()
    {
        mapScreenUI.SetActive(false);
        shopUI.SetActive(true);
    }

    public void SettingsButton()
    {
        mapScreenUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    public void BackToMapButton()
    {
        settingsUI.SetActive(false);
        shopUI.SetActive(false);
        guildUI.SetActive(false);
        mapScreenUI.SetActive(true);
    }

}
