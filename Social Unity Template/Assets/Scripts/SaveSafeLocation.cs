using System.Collections;
using System.Collections.Generic;
using Mapbox.Examples;
using Mapbox.Unity.Utilities;
using UnityEngine;

public class SaveSafeLocation : MonoBehaviour
{

    private SpawnOnMap _spawnOnMap;

    private ImmediatePositionWithLocationProvider _immediatePositionWithLocationProvider;
    // Start is called before the first frame update
    void Start()
    {
        _spawnOnMap = GameObject.FindWithTag("Spawner").GetComponent<SpawnOnMap>();
        _immediatePositionWithLocationProvider =
            GameObject.FindWithTag("Player").GetComponent<ImmediatePositionWithLocationProvider>();
        TakeLocationOfSafeAndSaveIt();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void TakeLocationOfSafeAndSaveIt()
    {
        var location = _immediatePositionWithLocationProvider.LocationProvider.CurrentLocation;
        string locationtoString = location.ToString();
        string test = "48.125257,15.262352";
        //_spawnOnMap._locationStrings.Add(locationtoString);
        _spawnOnMap._locationStrings.Add(test);
        Debug.Log(locationtoString);
    }
}
