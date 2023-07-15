using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Mapbox.Examples;
using Mapbox.Unity.Location;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Console = Mapbox.Unity.Utilities.Console;

public class Player : MonoBehaviour
{
    private List<double> distance;
    private SpawnOnMap _spawnOnMap;
    private ImmediatePositionWithLocationProvider _immediatePositionWithLocationProvider;
    private LocationArrayEditorLocationProvider _locationArrayEditorLocationProvider;
    private Quaternion _rotation;
    public TextMeshProUGUI debugText;
    public TextMeshProUGUI debugText2;

    
    public Faction faction { get; set; }
    public List<Friend> friends { get; set; }

    // TODO: Change MapPosition to whatever Position Property we use
    //private MapPosition _position { get; set; }

    public double money { get; set; }

    public int xp { get; set; }
    public const int xpRequiredForFactionChange = 2000; //Change Value if needed

    public List<ItemListElement> items { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        _spawnOnMap = GameObject.FindWithTag("Spawner").GetComponent<SpawnOnMap>();
        //debugText2.text = "BEFORE";
        _immediatePositionWithLocationProvider =
            GameObject.FindWithTag("Player").GetComponent<ImmediatePositionWithLocationProvider>();
        //_locationArrayEditorLocationProvider =
        //    GameObject.FindWithTag("EditorOnly").GetComponent<LocationArrayEditorLocationProvider>();
        _rotation = transform.rotation;
       //debugText2.text = "AFTER";

        SenPlayerPOLOZHENIE();
        //StartCoroutine(SendPlayerLocationAndRotationToServer());
        distance = new List<double>();
        //CalculateDistanceInEditor();
        //CalculateDistanceWithImmediatePosition();
        for (int i = 0; i < distance.Count; i++)
        {
            Debug.Log(distance[i] + " m");
        }

    }

    public void SenPlayerPOLOZHENIE()
    {
        //debugText2.text = "START COR";
        StartCoroutine(SendPlayerLocationAndRotationToServer());

    }

    // Update is called once per frame
    void Update()
    {
        //CalculateDistanceInEditor();
        //CalculateDistanceWithImmediatePosition();
        _rotation = transform.localRotation;
        //var locationX =
        //    _immediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude.x.ToString(
       //         CultureInfo.InvariantCulture).Replace(",", ".");
       
       // var locationY =
       //     _immediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude.y.ToString(
        //        CultureInfo.InvariantCulture).Replace(",", ".");
       // debugText.text = "X: " + locationX + " Y: " + locationY;
    }


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

    public IEnumerator SendPlayerLocationAndRotationToServer()
    {
       // debugText2.text = "IS CALLED 222222";

        yield return new WaitForSeconds(1f);
       // debugText2.text += "IS CALLED";
        while (true)
        {
            WWWForm form = new WWWForm();
            var locationX =
                _immediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude.x.ToString(
                    CultureInfo.InvariantCulture).Replace(",", ".");
       
            var locationY =
                _immediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude.y.ToString(
                    CultureInfo.InvariantCulture).Replace(",", ".");
           // debugText2.text += "BEFORE POST LocX: " + locationX + "LocY: " + locationY;

            form.AddField("locationX", locationX);
            form.AddField("locationY", locationY);
            form.AddField("rotation", _rotation.ToString().Replace(", ", ":").Trim('"').Trim('(').Trim(')'));

            using var www = new WWW(GameManager.Instance.BASE_URL + "send_location/", form);
            yield return www;
            //debugText2.text += www.text;
            Debug.Log("LocX: " + locationX + "LocY: " + locationY);
            yield return new WaitForSeconds(7f);
        }
    }

    private void CalculateDistanceWithImmediatePosition()
    {
        for (int i = 0; i < _spawnOnMap._locationStrings.Count; i++)
        {
            if (_spawnOnMap._locationStrings[i] == null)
            {
                continue;
            }
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
        for (int i = 0; i < _spawnOnMap._locationStrings.Count; i++)
        {
            if (_spawnOnMap._locationStrings[i] == null)
            {
                continue;
            }
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

    
}
