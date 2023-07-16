using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_Success : MonoBehaviour
{
    public TextMeshProUGUI message;
    private Animator anim;
    private Coroutine isPopping = null;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public void PopUp(string messageText)
    {
        message.text = messageText;
        if (isPopping == null)
        {
            isPopping = StartCoroutine(showMessage());
        }
    }

    public void resetMessage()
    {
        message.text = " ";
        isPopping = null;
    }

    public IEnumerator showMessage()
    {
        anim.Play("PopUp");
        yield return new WaitForSeconds(1f);
        anim.Play("Down");
        yield return new WaitForSeconds(0.25f);
        resetMessage();
    }
}
