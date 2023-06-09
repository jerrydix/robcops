using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Faction faction { get; set; }
    public List<Friend> friends { get; set; }

    // TODO: Change MapPosition to whatever Position Property we use
    //private MapPosition _position { get; set; }

    public double money { get; set; }

    public int xp { get; set; }
    public const int xpRequiredForFactionChange = 2000; //Change Value if needed

    public List<ItemListElement> items { get; set; }

    /**
     * Add the specified amount to the users money, negative values will subtract
     */
    public void ChangeMoney(double amount)
    {
        money += amount;
    }

    public void AddFriend(Friend neu)
    {
        if (friends.Contains(neu))
        {
            Debug.LogError("Friend already registerd!");
            return;
        }
        friends.Add(neu);
    }

    public void RemoveFriend(Friend toRemove)
    {
        if (!friends.Contains(toRemove))
        {
            Debug.LogError("Friend does not exist anyway!");
            return;
        }
        friends.Remove(toRemove);
    }

    public void SendFriendRequest()
    {
        // TODO: Add Friend Request Functionality
    }

    public void AddItem(Item item) 
    {
        foreach (ItemListElement i in items)
        {
            if(i.item.GetType() == item.GetType())
            {
                i.count++;
                return;
            }
        }
        items.Add(new ItemListElement(item));
    }

    public void RemoveItem(Item item) 
    {
        foreach(ItemListElement i in items)
        {
            if(i.item.GetType() == item.GetType())
            {
                i.count--;
                if(i.count == 0)
                {
                    items.Remove(i);
                }
                return;
            }        
        }
    }

    /**
     * Returns true if is Robber, false if is PoliceOfficer
     */
    public bool isRobber()
    {
        return faction.isRobber();
    }

    public bool canChangeFaction()
    {
        return xp >= xpRequiredForFactionChange;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
