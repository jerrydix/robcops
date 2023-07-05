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
        if (GameManager.Instance.role)
            SceneManager.LoadScene("Scenes/PoliceStation");
        else SceneManager.LoadScene("Scenes/Robunion");
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
        //GameManager.Instance.InitializeSafe();
    }

    public void SafePlaceCancelButton()
    {
        placeSafeButton.SetActive(true);
        safePlacingDialogue.SetActive(false);
    }

    public void PlaceLevel1SafeButton()
    {
        var cost = 10000;
        if (GameManager.Instance.money < cost) return;
        GameManager.Instance.InitializeSafe(1, cost);
        safePlacingDialogue.SetActive(false);
        placeSafeButton.SetActive(true);
    }

    public void PlaceLevel2SafeButton()
    {
        var cost = 100000;
        if (GameManager.Instance.money < cost) return;
        GameManager.Instance.InitializeSafe(2, cost);
        safePlacingDialogue.SetActive(false);
        placeSafeButton.SetActive(false);
    }

    public void PlaceLevel3SafeButton()
    {
        var cost = 250000;
        if (GameManager.Instance.money < cost) return;
        GameManager.Instance.InitializeSafe(3, cost);
        safePlacingDialogue.SetActive(false);
        placeSafeButton.SetActive(true);
    }


    public void PlaceLevel4SafeButton()
    {
        var cost = 1000000;
        if (GameManager.Instance.money < cost) return;
        GameManager.Instance.InitializeSafe(4, 1000000);
        safePlacingDialogue.SetActive(false);
        placeSafeButton.SetActive(true);
    }

    public void GetInfoButton()
    {
        GameManager.Instance.GetAllSafes();
    }

    //when police visible, when robber invisible
    public void ChangePlaceSafeButton(bool visible)
    {
        placeSafeButton.SetActive(visible);
    }
}