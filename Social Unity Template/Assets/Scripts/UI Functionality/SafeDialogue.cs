using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SafeDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI _safeDescriptionText;
    private TextMeshProUGUI _addressText;
    private TextMeshProUGUI _lobbyText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeSafe(string safeDescriptionText, string addressText, string lobbyText)
    {
        _safeDescriptionText.text = safeDescriptionText;
        _addressText.text = addressText;
        _lobbyText.text = lobbyText;
    }

    public void PressStartRobberyButton()
    {
        //todo StartMinigame etc.
    }

    public void PressCancelButton()
    {
        gameObject.SetActive(false);
    }
}
