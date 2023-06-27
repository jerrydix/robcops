using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchRoleDialogue: MonoBehaviour
{
    private string _roleText; 
    
    public void InitializeDialogue(string roleText)
    {
        _roleText = roleText;
    }

    public void PressSwitchRoleButton()
    {
        //todo SwitchRole etc.
    }

    public void PressCancelButton()
    {
        gameObject.SetActive(false);
    }
}
