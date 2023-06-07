using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPrintingMachine : MonoBehaviour
{
    private short maxLevel;
    private double[] moneyPrintingSpeedsPerLevel;   //Needs to be size of maxLevel
    private double[] moneyRequiredPerLevel;         //Needs to be size of maxLevel
    public double moneyPerHour { get; private set; }
    public short level { get; set; }
    public double moneyRequiredforNextLevel {get; private set; }

    public Boolean isMaxLevel()
    {
        return level == maxLevel;
    } 

    public void SpendMoney(double amount)
    {
        moneyRequiredforNextLevel -= amount;
        if(moneyRequiredforNextLevel <= 0)
        {
            level++;
            moneyPerHour = moneyPrintingSpeedsPerLevel[level];
            moneyRequiredforNextLevel -= moneyRequiredPerLevel[level] + moneyRequiredforNextLevel; //Reduce by overshooting amount. At this state moneyRequiredforNextLevel is either 0 or negative
        }
    }
}
