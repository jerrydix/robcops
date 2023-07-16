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

    [SerializeField] private SafeUIManager RURaidSafeUIManager;

    [SerializeField] private SwitchRoleDialogue switchRoleDialogue;
    [SerializeField] private GameObject mapScreenUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject penaltyUI;
    [SerializeField] private Button switchRoleButton;
    [SerializeField] private GameObject placeSafeButton;
    [SerializeField] private Image brassKnucklesImage;
    [SerializeField] private Sprite brassKnucklesSprite;
    [SerializeField] private TextMeshProUGUI brassKnucklesText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private GameObject safePlacingDialogue;
    [SerializeField] private GameObject corruptionDialogue;
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
    [SerializeField] private Image switchImage;
    [SerializeField] private Image switchImageBack;
    [SerializeField] private TextMeshProUGUI safeInfoText;
    public Sprite copIcon;
    public Sprite robIcon;

    private readonly int _cost1 = 10000;
    private readonly int _cost2 = 100000;
    private readonly int _cost3 = 250000;
    private readonly int _cost4 = 1000000;
    private int _currentCorruptionCopID;

    private int currentXP; //verbinden mit gamemanager

    private bool switchButtonActivated;

    private int XPthreshold = 100;
    // verbinden mit game manager
    //get current role gamemanager

    private void Start()
    {
        switchRoleButton.interactable = switchButtonActivated;
        //Debug.Log(GameManager.Instance.role);
        ChangePlaceSafeButton(GameManager.Instance.role);
        Debug.Log(GameManager.Instance.xp);
        switchImage.fillAmount = (float) GameManager.Instance.xp / XPthreshold;
        Debug.Log((float) GameManager.Instance.xp / XPthreshold);
        if (GameManager.Instance.role)
        {
            ChangeBrassKnucklesSpriteAndText();
        }
        else
        {
            ChangeSwitchIconToCop();
        }
        if (GameManager.Instance.xp >= XPthreshold)
        {
            switchButtonActivated = true;
            switchRoleButton.interactable = switchButtonActivated;
        }

        if (GameManager.Instance.role)
        {
            safeInfoText.gameObject.SetActive(true);
            StartCoroutine(setSafeMoneyText());
        }
        else
        {
            safeInfoText.gameObject.SetActive(false);
        }
        //safeUpdateRoutine = StartCoroutine(GameManager.Instance.UpdateSafes());
    }

    private void Update()
    {
        moneyText.text = GameManager.Instance.money.ToString();
        //todo same for xp
    }

    public void OnDestroy()
    {
        StopCoroutine(GameManager.Instance.updateSafesCoroutine);
    }

    public void ChangeSwitchIconToCop()
    {
        switchImageBack.sprite = copIcon;
        switchImage.sprite = copIcon;
    }

    public void ActivateDialogue(int level, double locationX, double locationY, bool createLobby, int id)
    {
        safeUIManager.gameObject.SetActive(true);
        safeUIManager.InitializeSafe(level, locationX, locationY, createLobby, id);
    }
    
    public void DeactivateDialogue()
    {
        safeUIManager.gameObject.SetActive(false);
        setSafeMoney();
    }

    public void ActivateRURaidDialogue(int level, double locationX, double locationY, bool createLobby, int id)
    {
        RURaidSafeUIManager.gameObject.SetActive(true);
        RURaidSafeUIManager.InitializeRURaidSafe(level, locationX, locationY, createLobby, id);
    }
    
    public void DeactivateRURaidDialogue()
    {
        RURaidSafeUIManager.gameObject.SetActive(false);
    }

    public void setSafeMoney()
    {
        StartCoroutine(setSafeMoneyText());
    }

    public IEnumerator updateSafeMoneyText()
    {
        while (true)
        {
            StartCoroutine(setSafeMoneyText());
            yield return new WaitForSeconds(30);
        }
    }

    public IEnumerator setSafeMoneyText()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "get_active_safes/");
        yield return www;
        safeInfoText.text = "Safes: " + www.text.Split("|")[0] + " | +" + www.text.Split("|")[1] + "/Hour";
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

    public void setXp()
    {
        StartCoroutine(SetXP());
    }

    public IEnumerator SetXP()
    {
        if (GameManager.Instance.role)
        {
            using var www = new WWW(GameManager.Instance.BASE_URL + "get_robberxp/");
            yield return www;
            int xp = int.Parse(www.text);
            GameManager.Instance.xp = xp;
            switchImage.fillAmount = (float) GameManager.Instance.xp / XPthreshold;
            if (GameManager.Instance.xp >= XPthreshold)
            {
                switchButtonActivated = true;
                switchRoleButton.interactable = switchButtonActivated;
            }
        }
        else
        {
            using var www = new WWW(GameManager.Instance.BASE_URL + "get_policexp/");
            yield return www;
            int xp = int.Parse(www.text);
            GameManager.Instance.xp = xp;
            switchImage.fillAmount = (float) GameManager.Instance.xp / XPthreshold;
            if (GameManager.Instance.xp >= XPthreshold)
            {
                switchButtonActivated = true;
                switchRoleButton.interactable = switchButtonActivated;
            } 
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
            script.SetParameters(i + 1 + ".", nennung, id, false, true);
        }
    }

    public void OpenPenalty(SafeManager safeManager)
    {
        penaltyUI.SetActive(true);
        penaltyUI.GetComponent<S_Penalty>().safeManager = safeManager;
    }

    public void ClosePenalty()
    {
        penaltyUI.SetActive(false);
        setXp();
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
        if (GameManager.Instance.xp >= 100)
        {
            switchRoleDialogue.gameObject.SetActive(true);
            switchRoleDialogue.InitializeDialogue(GameManager.Instance.role);
        }
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

    public void ActivateCorruptionDialogue(int copID)
    {
        _currentCorruptionCopID = copID;
        corruptionDialogue.SetActive(true);
    }

    public void PressGiveHintButton()
    {
        StartCoroutine(GiveHint());
    }

    private IEnumerator GiveHint()
    {
        var form = new WWWForm();
        form.AddField("cop_id", _currentCorruptionCopID);
        using var www = new WWW(GameManager.Instance.BASE_URL + "give_hint/", form);
        yield return www;
        if (!www.text.Split("|")[0].Equals("0"))
        {
            setXp();
            Debug.Log("Current robber xp: " + www.text);
            GameManager.Instance.successMessage.PopUp("You gave a cop a hint and you now have " + www.text + " Cop XP!");
            GameManager.Instance.xp = int.Parse(www.text);
            corruptionDialogue.SetActive(false);
        }
        else
        {
            GameObject.Find("Error").GetComponent<S_Error>().PopUp(www.text.Split("|")[1]);
        }
    }

    public void PressCorruptionDialogueCloseButton()
    {
        corruptionDialogue.SetActive(false);
    }
    
    public void CloseSafeInfoDialogue()
    {
        
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
        if (GameManager.Instance.money < _cost1)
        {
            GameManager.Instance.errorMessage.PopUp("You haven't got enough money. Have a good day!");
            return;
        }

        if (!GameManager.Instance.InitializeSafe(1, _cost1))
        {
            GameManager.Instance.errorMessage.PopUp("Cannot Spawn Safe on an occupied area, go somewhere else!");
            return;
        }
        GameManager.Instance.successMessage.PopUp("Successfully placed a Level 1 safe!");
        safePlacingDialogue.SetActive(false);
        placeSafeButton.SetActive(true);
    }

    public void PlaceLevel2SafeButton()
    {
        if (GameManager.Instance.money < _cost2)
        {
            GameManager.Instance.errorMessage.PopUp("You haven't got enough money. Have a good day!");
            return;
        }
        if (!GameManager.Instance.InitializeSafe(2, _cost2))
        {
            GameManager.Instance.errorMessage.PopUp("Cannot Spawn Safe on an occupied area, go somewhere else!");
            return;
        }
        GameManager.Instance.successMessage.PopUp("Successfully placed a Level 2 safe!");
        safePlacingDialogue.SetActive(false);
        placeSafeButton.SetActive(true);
    }

    public void PlaceLevel3SafeButton()
    {
        if (GameManager.Instance.money < _cost3)
        {
            GameManager.Instance.errorMessage.PopUp("You haven't have enough money. Have a good day!");
            return;
        }
        if (!GameManager.Instance.InitializeSafe(3, _cost3))
        {
            GameManager.Instance.errorMessage.PopUp("Cannot Spawn Safe on an occupied area, go somewhere else!");
            return;
        }
        GameManager.Instance.successMessage.PopUp("Successfully placed a Level 3 safe!");
        safePlacingDialogue.SetActive(false);
        placeSafeButton.SetActive(true);
    }


    public void PlaceLevel4SafeButton()
    {
        if (GameManager.Instance.money < _cost4)
        {
            GameManager.Instance.errorMessage.PopUp("You haven't enough money. Have a good day!");
            return;
        }
        if (!GameManager.Instance.InitializeSafe(4, _cost4))
        {
            GameManager.Instance.errorMessage.PopUp("Cannot Spawn Safe on an occupied area, go somewhere else!");
            return;
        }
        GameManager.Instance.successMessage.PopUp("Successfully placed a Level 4 safe!");
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

    public void ChangeBrassKnucklesSpriteAndText()
    {
        brassKnucklesImage.sprite = brassKnucklesSprite;
        brassKnucklesText.text = "POWER OF JUSTICE";
    }
}