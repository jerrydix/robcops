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
          Debug.Log(distances[i]);
        }

        
    }
    
    private void CalculateDistance()
    {
        for(int i = 0; i < _spawnOnMap._locationStrings.Length; i++)
        {
            var currentString = _spawnOnMap._locationStrings[i];
            var instance = Conversions.StringToLatLon(currentString);
            var x = Conversions.StringToLatLon(LocationArrayEditorLocationProvider._latitudeLongitude[0]);
            double playerLocation =  x.x;
            double playerLocationy = x.y;
            var deltaLat = (instance.x - playerLocation) * Mathd.PI / 180d;
            var deltaLon = (instance.y - playerLocationy) * Mathd.PI / 180d;
            var a = (Mathd.Pow(Mathd.Sin( deltaLat / 2d), 2d) + Mathd.Cos( playerLocation * Mathd.PI /180d)
                * Mathd.Cos(instance.x * Mathd.PI / 180d) * Mathd.Pow(Mathd.Sin( deltaLon / 2d), 2d));
            var c = 2d * Mathd.Atan2(Mathd.Sqrt(a), Mathd.Sqrt(1d - a));
            var result = 6371d * c;
            result *= 1000;
            var finalResult = Mathd.Abs(result);
            if (finalResult <= 1000)
            {
                distances.Add((int) finalResult);
            }
        }
    }
}
