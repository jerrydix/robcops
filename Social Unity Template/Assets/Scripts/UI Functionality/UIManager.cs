using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [FormerlySerializedAs("safeDialogue")] [FormerlySerializedAs("dialogue")] [SerializeField]
    private SafeUIManager safeUIManager;

    [SerializeField] private SwitchRoleDialogue switchRoleDialogue;
    [SerializeField] private GameObject mapScreenUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private Button switchRoleButton;
    [SerializeField] private GameObject placeSafeButton;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private GameObject safePlacingDialogue;
    [SerializeField] private GameObject robUnionListScreen;
    [SerializeField] private GameObject policeStationListScreen;
    [SerializeField] private GameObject guildUI;
    [SerializeField] private GameObject robPanel;
    [SerializeField] private GameObject copPanel;
    [SerializeField] private GameObject listElementPrefab;
    [SerializeField] private TextMeshProUGUI safeLvl1Text;
    [SerializeField] private TextMeshProUGUI safeLvl2Text;
    [SerializeField] private TextMeshProUGUI safeLvl3Text;
    [SerializeField] private TextMeshProUGUI safeLvl4Text;
    private readonly int _cost1 = 10000;
    private readonly int _cost2 = 100000;
    private readonly int _cost3 = 250000;
    private readonly int _cost4 = 1000000;

    private int currentXP; //verbinden mit gamemanager
    
    private bool switchButtonActivated;

    private int XPthreshold;
    // verbinden mit game manager
    //get current role gamemanager

    private void Start()
    {
        switchRoleButton.interactable = switchButtonActivated;
        Debug.Log(GameManager.Instance.role);
        ChangePlaceSafeButton(GameManager.Instance.role);
        
        //safeUpdateRoutine = StartCoroutine(GameManager.Instance.UpdateSafes());
    }

    private void Update()
    {
        moneyText.text = GameManager.Instance.money.ToString();
        //todo same for xp
    }

    public void ActivateDialogue(int level, double locationX, double locationY, bool createLobby, int id)
    {
        safeUIManager.gameObject.SetActive(true);
        safeUIManager.InitializeSafe(level, locationX, locationY, createLobby, id);
    }

    public void GuildButton()
    {
        if (GameManager.Instance.guild != -1)
        {
            if (GameManager.Instance.role)
                SceneManager.LoadScene("Scenes/PoliceStation");
            else SceneManager.LoadScene("Scenes/Robunion");
        }
        else
        {
            guildUI.SetActive(true);
            if (GameManager.Instance.role)
            {
                policeStationListScreen.SetActive(true);
                deleteChildrenPanelCop();
                StartCoroutine(getPoliceStations());
            }
            else
            {
                robUnionListScreen.SetActive(true);
                deleteChildrenPanelRob();
                StartCoroutine(getRobUnions());
            }
        }
    }

    public void deleteChildrenPanelRob()
    {
        foreach (Transform child in robPanel.transform) Destroy(child.gameObject);
    }

    public void OnDestroy()
    {
        StopCoroutine(GameManager.Instance.updateSafesCoroutine);
    }

    public void deleteChildrenPanelCop()
    {
        foreach (Transform child in copPanel.transform) Destroy(child.gameObject);
    }

    public IEnumerator getRobUnions()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "get_all_robunions/");
        yield return www;
        var robunions = www.text.Split("|");
        for (var i = 0; i < robunions.Length; i++)
        {
            var element = Instantiate(listElementPrefab, robPanel.transform);
            var script = element.GetComponent<S_ListElement>();
            var id = int.Parse(robunions[i].Split(", ")[0]);
            var nennung = robunions[i].Split(", ")[1];
            script.SetParameters(i + 1 + ".", nennung, id, false, false);
        }
    }

    public IEnumerator getPoliceStations()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "get_all_stations/");
        yield return www;
        var stations = www.text.Split("|");
        for (var i = 0; i < stations.Length; i++)
        {
            var element = Instantiate(listElementPrefab, copPanel.transform);
            var script = element.GetComponent<S_ListElement>();
            var id = int.Parse(stations[i].Split(", ")[0]);
            var nennung = stations[i].Split(", ")[1];
            script.SetParameters(i + 1 + ".", nennung, id, false, false);
        }
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
        deleteChildrenPanelRob();
        deleteChildrenPanelCop();
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
        placeSafeButton.SetActive(false);
        safePlacingDialogue.SetActive(true);
        safeLvl1Text.text = DisplayMoney(_cost1);
        safeLvl2Text.text = DisplayMoney(_cost2);
        safeLvl3Text.text = DisplayMoney(_cost3);
        safeLvl4Text.text = DisplayMoney(_cost4);
        //GameManager.Instance.InitializeSafe();
    }

    public string DisplayMoney(int money)
    {
        var result = "";
        if (money >= 1000000)
        {
            var amount_H = money / 1000000f;
            var show = Mathf.Round(amount_H * 10f) / 10f;
            result = show + "M";
        }
        else if (money >= 1000)
        {
            var amount_H = money / 1000f;
            var show = Mathf.Round(amount_H * 10f) / 10f;
            result = show + "K";
        }
        else
        {
            result = "" + money;
        }

        return result;
    }

    public void SafePlaceCancelButton()
    {
        placeSafeButton.SetActive(true);
        safePlacingDialogue.SetActive(false);
    }

    public void PlaceLevel1SafeButton()
    {
        if (GameManager.Instance.money < _cost1) return;
        GameManager.Instance.InitializeSafe(1, _cost1);
        safePlacingDialogue.SetActive(false);
        placeSafeButton.SetActive(true);
    }

    public void PlaceLevel2SafeButton()
    {
        if (GameManager.Instance.money < _cost2) return;
        GameManager.Instance.InitializeSafe(2, _cost2);
        safePlacingDialogue.SetActive(false);
        placeSafeButton.SetActive(false);
    }

    public void PlaceLevel3SafeButton()
    {
        if (GameManager.Instance.money < _cost3) return;
        GameManager.Instance.InitializeSafe(3, _cost3);
        safePlacingDialogue.SetActive(false);
        placeSafeButton.SetActive(true);
    }


    public void PlaceLevel4SafeButton()
    {
        if (GameManager.Instance.money < _cost4) return;
        GameManager.Instance.InitializeSafe(4, _cost4);
        safePlacingDialogue.SetActive(false);
        placeSafeButton.SetActive(true);
    }

    public void GetInfoButton()
    {
        //GameManager.Instance.GetAllSafes();
    }

    //when police visible, when robber invisible
    public void ChangePlaceSafeButton(bool visible)
    {
        placeSafeButton.SetActive(visible);
    }
}