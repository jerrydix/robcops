using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{

    public List<Item> items {get; set;}

    public void AddItem(Item neu)
    {
        items.Add(neu);
    }

    public void RemoveItem(Item toRemove)
    {
        items.Remove(toRemove);
    }

    public void BuyItem(Player buyer, Item toBuy)
    {
        if(buyer.money >= toBuy.price)
        {
            buyer.AddItem(toBuy);
            buyer.ChangeMoney(-toBuy.price);
        } else
        {
            Debug.Log("Player has not enough money");
        }
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
