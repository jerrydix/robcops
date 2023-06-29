using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [FormerlySerializedAs("dialogue")] [SerializeField] private SafeDialogue safeDialogue;
    [SerializeField] private SwitchRoleDialogue switchRoleDialogue;
    [SerializeField] private GameObject mapScreenUI;
    [SerializeField] private GameObject guildUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private Button switchRoleButton;

    private int currentXP; //verbinden mit gamemanager
    private int XPthreshold;
    private bool switchButtonActivated;
    
    // verbinden mit game manager
    //get current role gamemanager

    private void Start()
    {
        switchRoleButton.interactable = switchButtonActivated;
    }

    public void ActivateDialogue(string safeDescriptionText, string addressText, string lobbyText)
    {
        safeDialogue.gameObject.SetActive(true);
        safeDialogue.InitializeSafe(safeDescriptionText, addressText, lobbyText);
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

    public void AddXP(int amount)
    {
        currentXP += amount;
        if (currentXP >= XPthreshold)
        {
            switchButtonActivated = true;
            switchRoleButton.interactable = switchButtonActivated;
        }     
    }

    public void SwitchRoleButton()
    {
        switchRoleDialogue.gameObject.SetActive(true);
        //switchRoleDialogue.InitializeDialogue(GameManager.instance.currentRole);
    }

    public void PlaceSafeButton()
    {
        GameManager.Instance.InitializeSafe();
    }

    public void GetInfoButton()
    {
        GameManager.Instance.GetAllSafes();
    }
}