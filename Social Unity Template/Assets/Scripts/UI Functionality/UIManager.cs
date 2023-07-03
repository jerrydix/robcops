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

    private int currentXP; //verbinden mit gamemanager
    private bool switchButtonActivated;
    private int XPthreshold;

    // verbinden mit game manager
    //get current role gamemanager

    private void Start()
    {
        switchRoleButton.interactable = switchButtonActivated;
        ChangePlaceSafeButton(GameManager.Instance.role);
    }

    public void ActivateDialogue(int level, double locationX, double locationY, bool createLobby, int id)
    {
        safeUIManager.gameObject.SetActive(true);
        safeUIManager.InitializeSafe(level, locationX, locationY, createLobby, id);
    }

    public void GuildButton()
    {
        SceneManager.LoadScene("Scenes/Robunion");
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
        GameManager.Instance.InitializeSafe();
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