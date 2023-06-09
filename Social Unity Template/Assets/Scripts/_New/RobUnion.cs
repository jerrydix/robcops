using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobUnion : Guild
{
    public MoneyPrintingMachine moneyPrintingMachine { get; set; }

    public void GiveMoneyToMembers()
    {
        foreach (Player member in members)
        {
            //Implement add money to members when 1 day has passed
        }
    }
}
