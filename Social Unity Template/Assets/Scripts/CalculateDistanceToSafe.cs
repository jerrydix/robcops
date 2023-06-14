using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Examples;
using Mapbox.Unity.Location;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class CalculateDistanceToSafe : MonoBehaviour
{
    private List<double> distances;

    private SpawnOnMap _spawnOnMap;
    private ImmediatePositionWithLocationProvider ImmediatePositionWithLocationProvider;
    private LocationArrayEditorLocationProvider LocationArrayEditorLocationProvider;
    private void Awake()
    {
        _spawnOnMap = GameObject.FindWithTag("Spawner").GetComponent<SpawnOnMap>();
        ImmediatePositionWithLocationProvider =
            GameObject.FindWithTag("Player").GetComponent<ImmediatePositionWithLocationProvider>();
        LocationArrayEditorLocationProvider =
            GameObject.FindWithTag("EditorOnly").GetComponent<LocationArrayEditorLocationProvider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        distances = new List<double>();
        CalculateDistance();
       
        for (int i = 0; i < distances.Count; i++)
        {
          //Debug.Log(distances[i]);
        }

        
    }
    
    private void CalculateDistance()
    {
        for(int i = 0; i < _spawnOnMap._locationStrings.Length; i++)
        {
            var currentString = _spawnOnMap._locationStrings[i];
            var instance = Conversions.StringToLatLon(currentString);
            //Debug.Log(instance.x);
            //var distanceObject = Conversions.LatLonToMeters(instance);
            //Debug.Log(distanceObject);
            var x = Conversions.StringToLatLon(LocationArrayEditorLocationProvider._latitudeLongitude[0]);
            float playerLocation =(float) x.x;
            Debug.Log(playerLocation);
            float playerLocationy =(float) x.y;
            //Debug.Log(playerLocation);
            //Debug.Log(playerLocationy);
            //var distancePlayer = Conversions.LatLonToMeters(playerLocation.CurrentLocation.LatitudeLongitude);
            //Debug.Log(distancePlayer);

            var deltaLat = ((float)instance.x - playerLocation) * Mathf.PI / 180;
            var deltaLon = ((float)instance.y - playerLocationy) * Mathf.PI / 180;

            var a = (Mathf.Pow(Mathf.Sin( deltaLat / 2), 2) + Mathf.Cos( playerLocation * Mathf.PI /180)
                * Mathf.Cos((float) instance.x * Mathf.PI / 180) * Mathf.Pow(Mathf.Sin((float) deltaLon / 2), 2));
            var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
            var result = 6371f * c;
            //result *= 1000;
            var finalResult = Mathf.Abs(result);
            //var test = new Vector2d(distanceObject.x - distancePlayer.y, distanceObject.y - distancePlayer.y);
            Debug.Log(a);
            Debug.Log(c);
            Debug.Log(result);
            Debug.Log(finalResult);
            //double calcDistance = (distanceObject - distancePlayer).sqrMagnitude;
            distances.Add(finalResult);
        }
    }
}
