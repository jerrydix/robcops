using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Examples;
using Mapbox.Map;
using Mapbox.Unity.Location;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;

public class CalculateDistanceToSafe : MonoBehaviour
{
    private List<double> distances;

    private SpawnOnMap _spawnOnMap;
    private ImmediatePositionWithLocationProvider _immediatePositionWithLocationProvider;
    private LocationArrayEditorLocationProvider _locationArrayEditorLocationProvider;
    private void Awake()
    {
        _spawnOnMap = GameObject.FindWithTag("Spawner").GetComponent<SpawnOnMap>();
        _immediatePositionWithLocationProvider =
            GameObject.FindWithTag("Player").GetComponent<ImmediatePositionWithLocationProvider>();
        _locationArrayEditorLocationProvider =
            GameObject.FindWithTag("EditorOnly").GetComponent<LocationArrayEditorLocationProvider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        distances = new List<double>();
        CalculateDistanceInEditor();
        CalculateDistanceWithImmediatePosition();

        for (int i = 0; i < distances.Count; i++)
        {
          Debug.Log(distances[i] + " m");
        }

        
    }

    void Update()
    {
        CalculateDistanceInEditor();
        CalculateDistanceWithImmediatePosition();
        
    }

    private void CalculateDistanceWithImmediatePosition()
    {
        for (int i = 0; i < _spawnOnMap._locationStrings.Length; i++)
        {
            //Get Locations of Safes and Player
            var currentString = _spawnOnMap._locationStrings[i];
            var instance = Conversions.StringToLatLon(currentString);
            var playerLocation = _immediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude;
            double playerLocationX = playerLocation.x;
            double playerLocationY = playerLocation.y;
            //Calculate the Distance
            var deltaLat = (instance.x - playerLocationX) * Mathd.PI / 180;
            var deltaLon = (instance.y - playerLocationY) * Mathd.PI / 180;
            var calc = (Mathd.Pow(Mathd.Sin(deltaLat / 2), 2) + Mathd.Cos(playerLocationX * Mathd.PI / 180)
                * Mathd.Cos(instance.x * Mathd.PI / 180) * Mathd.Pow(Mathd.Sin(deltaLon / 2),2));
            var temp = 2 * Mathd.Atan2(Mathd.Sqrt(calc), Mathd.Sqrt(1 - calc));
            var result = 6371d * temp;
            result *= 1000;
            var finalResult = Mathd.Abs(result);
            //Filter Safes that are more than 1km away
            if (finalResult < 1000)
            {
                distances.Add((int) finalResult);
            }
        }
    }
    
    private void CalculateDistanceInEditor()
    {
        for(int i = 0; i < _spawnOnMap._locationStrings.Length; i++)
        {
            //Get Locations of Safes and Player
            var currentString = _spawnOnMap._locationStrings[i];
            var instance = Conversions.StringToLatLon(currentString);
            var x = Conversions.StringToLatLon(_locationArrayEditorLocationProvider._latitudeLongitude[0]);
            double playerLocation =  x.x;
            double playerLocationy = x.y;
            //Calculate the Distance
            var deltaLat = (instance.x - playerLocation) * Mathd.PI / 180d;
            var deltaLon = (instance.y - playerLocationy) * Mathd.PI / 180d;
            var a = (Mathd.Pow(Mathd.Sin( deltaLat / 2d), 2d) + Mathd.Cos( playerLocation * Mathd.PI /180d)
                * Mathd.Cos(instance.x * Mathd.PI / 180d) * Mathd.Pow(Mathd.Sin( deltaLon / 2d), 2d));
            var c = 2d * Mathd.Atan2(Mathd.Sqrt(a), Mathd.Sqrt(1d - a));
            var result = 6371d * c;
            result *= 1000;
            var finalResult = Mathd.Abs(result);
            //Filter Safes that are more than 1km away
            if (finalResult <= 1000)
            {
                distances.Add((int) finalResult);
            }
        }
    }
    
    
}
