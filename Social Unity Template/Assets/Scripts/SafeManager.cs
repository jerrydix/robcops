using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeManager : MonoBehaviour
{
    [SerializeField] private Canvas safeUI;

    public int level;
    public int id;
    public int hp; //todo make ranges of hp depending on level
    public double locationX;
    public double locationY;
    public int status;
    public bool createLobby;
    public int lobbyPlayerCount;
    public List<string> lobbyNames;
    private UIManager _uiManager;
    private GameObject player;
    public GameObject safeModel;
    public GameObject robuionModel;
    public int isRobUnion;
    

    // Start is called before the first frame update
    private void Awake()
    {
        safeUI.worldCamera = Camera.main;
        createLobby = true;
    }

    private void Start()
    {
        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        player = GameObject.FindWithTag("Player");
    }

    public void ActivateRobUnion()
    {
        robuionModel.SetActive(true);
        safeModel.SetActive(false);
    }
    
    public void DeactivateRobUnion()
    {
        robuionModel.SetActive(false);
        safeModel.SetActive(true);

    }

    public void SafeClicked()
    {
        Debug.Log("safe clicked");
        //todo add checkSafe method
        Debug.Log("SAFE ID: " + id);
        if (!GameManager.Instance.role && getDistanceToObject() <= 20f && status != 3 && isRobUnion == 0)
            StartCoroutine(checkBreakInStatus());
        if (GameManager.Instance.role && isRobUnion == 1 && getDistanceToObject() <= 20f)
        {
            StartCoroutine(GameManager.Instance.getRobUnionSafeID());
            if (GameManager.Instance.currentRobUnionSafeID == id)
            {
                StartCoroutine(checkRURaidStatus());
            }
        }
        if (getDistanceToObject() <= 20f && status == 3 && GameManager.Instance.role)
        {
            _uiManager.OpenPenalty(this);
        }
    }

    public float getDistanceToObject()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        return dist;
    }

    public void arrest(int penalty)
    {
        _uiManager.ClosePenalty();
        if (penalty == 0)
        {
            StartCoroutine(giveXP());
        }
        StartCoroutine(Arrest(penalty));
    }
    
    private IEnumerator giveXP()
    {
        WWWForm form = new WWWForm();
        form.AddField("xp", 50);
        using var www = new WWW(GameManager.Instance.BASE_URL + "edit_robberxp" + "/", form);
        yield return www;
        Debug.Log("Current XP: " + www.text);
        GameManager.Instance.xp = int.Parse(www.text);
    }

    public IEnumerator Arrest(int penalty)
    {
        WWWForm form = new WWWForm();
        form.AddField("safeId", id);
        form.AddField("penalty", penalty);
        Debug.Log("Id " + id + "penalty " + penalty);
        using var www = new WWW(GameManager.Instance.BASE_URL + "arrest_lobby/", form);
        yield return www;
        StartCoroutine(GameManager.Instance.GetPlayerMoneyOnce());
        Debug.Log(www.text);
    }

    private IEnumerator checkBreakInStatus()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "checkLobby/" + id + "/");
        yield return www;
        Debug.Log(GameManager.Instance.BASE_URL + "checkLobby/" + id + "/");
        createLobby = Convert.ToBoolean(int.Parse(www.text));
        _uiManager.ActivateDialogue(level, locationX, locationY, createLobby, id);
    }
    
    private IEnumerator checkRURaidStatus()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "checkLobby/" + id + "/");
        yield return www;
        Debug.Log(GameManager.Instance.BASE_URL + "checkLobby/" + id + "/");
        createLobby = Convert.ToBoolean(int.Parse(www.text));
        _uiManager.ActivateRURaidDialogue(level, locationX, locationY, createLobby, id);
    }
}