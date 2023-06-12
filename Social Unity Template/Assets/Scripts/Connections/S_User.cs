using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_User : MonoBehaviour
{
    private string username;
    private int money;
    private int amountOfClicks;
    private float clickPower;
    private double locationX;
    private double locationY;
    private bool role;
    private int guildID;

    public void SetData(ArrayList list)
    {
        username = (string) list[1];
        money = (int) list[2];
        amountOfClicks = (int) list[3];
        clickPower = (float) list[4];
        locationX = (double) list[5];
        locationY = (double) list[6];
        role = (bool) list[7];
        guildID = (int) list[8];
    }
}
