using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceOfficer : Faction
{
    public List<Safe> safes;

    public override bool isRobber()
    {
        return false;
    }

    public void PlaceSafe()
    {
        //TODO: Implement
    }

}
