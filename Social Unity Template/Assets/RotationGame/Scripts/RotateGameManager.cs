using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateGameManager : MonoBehaviour
{
    public float hp { get; private set; }
    public float time { get; private set; }

    public float damage { get; private set; }

    //todo: connect with gloabal time and hp

    [SerializeField]
    private Text timeField;
    [SerializeField]
    private Text eventField;
    [SerializeField]
    private Text hpField;

    [SerializeField] private Maze[] scenes;

    private GameObject gameBall;
    private GameObject startPos;
    private GameObject scene;

    private bool hasEnded;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Time.timeScale = 0;
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);

        for (short i = 0; i < scenes.Length; i++)
        {
            scenes[i].scene.SetActive(false);
        }
    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    public void Initialize(float time, float hp, float damage)
    {
        this.time = time;
        this.hp = hp;
        this.damage = damage;
        timeField.text = time.ToString("F2");
        hpField.text = hp.ToString("F2");
        eventField.text = "";
        hasEnded = false;
        PickRandomScene();
    }

    public void ResetBall()
    {
        gameBall.transform.position = startPos.transform.position;
    }

    

    private void FixedUpdate()
    {
        if (!hasEnded)
        {
            time -= Time.fixedDeltaTime * Time.timeScale;
            timeField.text = time.ToString("F2");
            if (time <= 0)
            {
                Fail();
                hasEnded = true;
                time = 0;
                timeField.text = time.ToString("F2");
            }
        }

    }

    public void DecrementHealth()
    {
        if (!hasEnded) {
            hp -= damage;
            hpField.text = hp.ToString("F2");

            if (hp <= 0)
            {
                Win();
                hasEnded = true;
            } else
            {
                scene.SetActive(false);
                PickRandomScene();
            }
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

    private IEnumerator StartGame()
    {
        yield return new WaitForSecondsRealtime(1);
        eventField.text = "3";
        yield return new WaitForSecondsRealtime(1);
        eventField.text = "2";
        yield return new WaitForSecondsRealtime(1);
        eventField.text = "1";
        yield return new WaitForSecondsRealtime(1);
        eventField.text = "";
        Time.timeScale = 1;
    }

    private void PickRandomScene()
    {
        int rand = UnityEngine.Random.Range(0, scenes.Length);
        Maze s = scenes[rand];
        scene = s.scene;
        startPos = s.ballStart;
        gameBall = s.ball;
        scene.SetActive(true);
        ResetBall();
    }
}
