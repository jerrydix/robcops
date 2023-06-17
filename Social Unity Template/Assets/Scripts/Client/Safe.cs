using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Safe : MonoBehaviour //Make Subclass for each Safe Level
{
    public short level { get; protected set; }
    public float moneyPerDay { get; protected set; }

    public float timeSincePlaced { get; protected set; } // in s 

    public float minTimeRequired { get; protected set; } // in s

    public Safe()
    { 
        timeSincePlaced = 0;
    }

    private void FixedUpdate()
    {
        timeSincePlaced += Time.fixedDeltaTime;
    }

    public bool hasMinTimeExpired()
    {
        return timeSincePlaced >= minTimeRequired;
    }

    public float getExpectedMoneyPolice() //return the amount of money the safe will give a Police Ofiicer if they would claim it now
    {
        if(!hasMinTimeExpired()) { return 0.0f; }
        return moneyPerDay * (int)(timeSincePlaced / (60 * 60 * 24));
    }

    public float getExpectedMoneyRobber()
    {
        return moneyPerDay * ((int)(timeSincePlaced / (60 * 60 * 24)) + 1); //return the amount of money the safe will give a Robber if they would crack it now
    }
}
