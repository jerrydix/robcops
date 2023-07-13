using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwitchRoleDialogue: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roleText;
    
    public void InitializeDialogue(bool role)
    {
        if (role)
        {
            _roleText.text = "Switch to Robber?";
        }
        else
        {
            _roleText.text = "Switch to Cop?";
        }
    }

    public void PressSwitchRoleButton()
    {
        StartCoroutine(SwitchRole());
    }

    private IEnumerator SwitchRole()
    {
        using var www = new WWW(GameManager.Instance.BASE_URL + "switch_role/");
        yield return www;
        yield return StartCoroutine(GameManager.Instance.getPlayerInfo());
        gameObject.SetActive(false);
    }

    public void PressCancelButton()
    {
        gameObject.SetActive(false);
    }
}
