using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_User : MonoBehaviour //not used
{   
    private string username;
    private int money;
    private int amountOfClicks;
    private float clickPower;
    private Vector2 location = new Vector2();
    private bool role;
    private Guild guild;

    public void SetData(List<string> list)
    {
        username = list[0];
        money = int.Parse(list[1]);
        amountOfClicks = int.Parse(list[2]);
        clickPower = float.Parse(list[3]);
        location = new Vector2(float.Parse(list[4]), float.Parse(list[5]));
        role = bool.Parse(list[6]);
        //guild = int.Parse(list[7]);
    }
}
