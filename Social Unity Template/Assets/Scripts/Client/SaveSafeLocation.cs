using System.Collections;
using System.Collections.Generic;
using Mapbox.Examples;
using Mapbox.Unity.Location;
using Mapbox.Unity.Utilities;
using UnityEngine;

public class SaveSafeLocation : MonoBehaviour
{

    private SpawnOnMap _spawnOnMap;

    private ImmediatePositionWithLocationProvider _immediatePositionWithLocationProvider;

    private LocationArrayEditorLocationProvider _locationArrayEditorLocationProvider;
    // Start is called before the first frame update
    void Start()
    {
        _locationArrayEditorLocationProvider =
            GameObject.FindWithTag("EditorOnly").GetComponent<LocationArrayEditorLocationProvider>();
        _spawnOnMap = GameObject.FindWithTag("Spawner").GetComponent<SpawnOnMap>();
        _immediatePositionWithLocationProvider =
            GameObject.FindWithTag("Player").GetComponent<ImmediatePositionWithLocationProvider>();
        TakeLocationOfSafeAndSaveIt();
        //TakeLocationOfSafeInEditorandSaveIt();
    }

    // Update is called once per frame
    void Update()
    {
        //TakeLocationOfSafeInEditorandSaveIt();
        TakeLocationOfSafeAndSaveIt();
    }


    private void TakeLocationOfSafeInEditorandSaveIt()
    {
        var location = _locationArrayEditorLocationProvider.CurrentLocation.LatitudeLongitude;
        string test = location.x + "," + location.y;
        Debug.Log(test);
    }


    private void TakeLocationOfSafeAndSaveIt()
    {
        var location = _immediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude;
        string locationtoString = location.x + "," + location.y;
        Debug.Log(locationtoString);
    }
}
