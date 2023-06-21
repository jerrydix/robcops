using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Examples;
using Mapbox.Unity.Location;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using Console = Mapbox.Unity.Utilities.Console;

public class Player : MonoBehaviour
{
    private List<double> distance;
    private SpawnOnMap _spawnOnMap;
    private ImmediatePositionWithLocationProvider _immediatePositionWithLocationProvider;
    private LocationArrayEditorLocationProvider _locationArrayEditorLocationProvider;
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

    private void CalculateDistanceWithImmediatePosition()
    {
        for (int i = 0; i < _spawnOnMap._locationStrings.Length; i++)
        {
            // Get Location of Safe and Player
            var currentString = _spawnOnMap._locationStrings[i];
            var instance = Conversions.StringToLatLon(currentString);
            var playerLocation = _immediatePositionWithLocationProvider.LocationProvider.CurrentLocation
                .LatitudeLongitude;
            double playerLocationX = playerLocation.x;
            double playerLocationY = playerLocation.y;
            
            //Calculate the Distance

            var deltaLat = (instance.x - playerLocationX) * Mathd.PI / 180;
            var deltaLon = (instance.y - playerLocationY) * Mathd.PI / 180;

            var calc = (Mathd.Pow(Mathd.Sin(deltaLat / 2), 2)
                        + Mathd.Cos(playerLocationX * Mathd.PI / 180) * Mathd.Cos(instance.x * Mathd.PI / 180) *
                        Mathd.Pow(Mathd.Sin(deltaLon / 2), 2));
            var temp = 2 * Mathd.Atan2(Mathd.Sqrt(calc), Mathd.Sqrt(1 - calc));
            var result = 6371d * temp;
            result *= 1000;
            var finalResult = Mathd.Abs(result);
            
            //Filter Safes that are more than 1km away

            if (finalResult < 1000)
            {
                distance.Add((int) finalResult);
            }
        }
    }

    private void CalculateDistanceInEditor()
    {
        for (int i = 0; i < _spawnOnMap._locationStrings.Length; i++)
        {
            //Get Locations of Safe and Player

            var currentString = _spawnOnMap._locationStrings[i];
            var instance = Conversions.StringToLatLon(currentString);
            var x = Conversions.StringToLatLon(_locationArrayEditorLocationProvider._latitudeLongitude[0]);
            double playerLocation = x.x;
            double playerLocationy = x.y;
            
            //Calculate the Distance

            var deltaLat = (instance.x - playerLocation) * Mathd.PI / 180;
            var deltaLon = (instance.y - playerLocationy) * Mathd.PI / 180;

            var calc = (Mathd.Pow(Mathd.Sin(deltaLat / 2), 2) + Mathd.Cos(playerLocation * Mathd.PI / 180)
                * Mathd.Cos(instance.x * Mathd.PI / 180) * Mathd.Pow(Mathd.Sin(deltaLon / 2), 2));
            var temp = 2 * Mathd.Atan2(Mathd.Sqrt(calc), Mathd.Sqrt(1 - calc));
            var result = 6371 * temp;
            result *= 1000;
            var finalResult = Mathd.Abs(result);
            
            //Filter Safes that are more than 1km away

            if (finalResult <=  1000)
            {
                distance.Add((int) finalResult);
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        _spawnOnMap = GameObject.FindWithTag("Spawner").GetComponent<SpawnOnMap>();
        _immediatePositionWithLocationProvider =
            GameObject.FindWithTag("Player").GetComponent<ImmediatePositionWithLocationProvider>();
        _locationArrayEditorLocationProvider =
            GameObject.FindWithTag("EditorOnly").GetComponent<LocationArrayEditorLocationProvider>();
        distance = new List<double>();
        CalculateDistanceInEditor();
        CalculateDistanceWithImmediatePosition();
        for (int i = 0; i < distance.Count; i++)
        {
            Debug.Log(distance[i] + " m");
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateDistanceInEditor();
        CalculateDistanceWithImmediatePosition();
        
    }
    
}
