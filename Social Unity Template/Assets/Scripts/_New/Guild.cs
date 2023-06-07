using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public abstract class Guild : MonoBehaviour
{
    public List<Player> members { get; set; }

    public Store store { get; set; }

    public void AddMember(Player neu)
    {
        if (members.Contains(neu))
        {
            Debug.LogError("Member is already present");
            return;
        }
        members.Add(neu);
    }

    public void RemoveMember(Player toRemove)
    {
        if (!members.Contains(toRemove))
        {
            Debug.Log("Member is not present and cant be removed");
            return;
        }
        members.Remove(toRemove);
    }

}
