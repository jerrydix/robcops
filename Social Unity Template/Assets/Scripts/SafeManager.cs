using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeManager : MonoBehaviour
{
    [SerializeField] private Canvas safeUI;
    private UIManager _uiManager;
    
    private S_UserLogin client;

    public int level;
    public int id;
    public int hp; //todo make ranges of hp depending on level
    public double locationX;
    public double locationY;

    // Start is called before the first frame update
    void Awake()
    {
        safeUI.worldCamera = Camera.main;
    }

    private void Start()
    {
        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();
    }
    
    public void SafeClicked()
    {
        Debug.Log("safe clicked");
        _uiManager.ActivateDialogue(level, locationX, locationY);
    }
}
