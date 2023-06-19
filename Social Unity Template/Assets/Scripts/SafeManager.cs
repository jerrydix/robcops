using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeManager : MonoBehaviour
{
    [SerializeField] private Canvas safeUI;
    private UIManager _uiManager;
    
    private string _safeDescriptionText;
    private string _addressText;
    private string _lobbyText;

    // Start is called before the first frame update
    void Awake()
    {
        safeUI.worldCamera = Camera.main;
    }

    private void Start()
    {
        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SafeClicked()
    {
        Debug.Log("safe clicked");
        _uiManager.ActivateDialogue(_safeDescriptionText, _addressText, _lobbyText);
    }
}
