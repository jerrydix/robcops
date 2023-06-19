using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private SafeDialogue dialogue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateDialogue(string safeDescriptionText, string addressText, string lobbyText)
    {
        dialogue.gameObject.SetActive(true);
        dialogue.InitializeSafe(safeDescriptionText, addressText, lobbyText);
    }
}
