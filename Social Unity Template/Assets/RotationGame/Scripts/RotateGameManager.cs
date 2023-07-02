using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateGameManager : MonoBehaviour
{
    public float hp { get; set; }
    public float damage { get; set;}
    public float time { get; set; }

    [SerializeField]
    private GameObject gameBall;
    [SerializeField]
    private GameObject startPos;

    [SerializeField]
    private Text timeField;
    [SerializeField]
    private Text eventField;
    [SerializeField]
    private Text hpField;

    public void Initialize(float time, float hp, float damage)
    {
        this.time = time;
        this.hp = hp;
        this.damage = damage;
        ResetBall();
        timeField.text = time.ToString("F2");
        hpField.text = hp.ToString("F2");
        eventField.text = "";
    }

    public void ResetBall()
    {
        gameBall.transform.position = startPos.transform.position;
    }

    private void FixedUpdate()
    {
        time -= Time.fixedDeltaTime;
        timeField.text = time.ToString("F2");
        hpField.text = hp.ToString("F2");
        if (hp <= 0)
        {
            Win();
        } else if (time <= 0)
        {
            Fail();
        }

    }

    private void Fail()
    {
        eventField.text = "You Failed!";
    }

    private void Win()
    {
        eventField.text = "You Won!";
    }
}
