using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : MonoBehaviour
{
    public Player friend { get; set; }

    //Some Metadata e.g. Frienshiprank, Friendshiptime

    public override bool Equals(object other)
    {
        return other is Player && friend.Equals((Player)other);
    }

}
