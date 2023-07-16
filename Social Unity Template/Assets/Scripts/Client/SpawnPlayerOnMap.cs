using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;

public class SpawnPlayerOnMap : MonoBehaviour
{
    public GameManager GameManager;
    private List<string> playersLocation = new List<string>();
    private List<string> spawnedPlayers;
    private List<Vector2d> _locations;
    private List<GameObject> _spawnedObject;
    public GameObject _gameObject;
    [SerializeField] private AbstractMap _map;
    [SerializeField] private float _spawnScale = 100f;
    private bool canI;

    private void Start()
    {
        GameManager = GameObject.FindWithTag("GM").GetComponent<GameManager>();
        StartCoroutine(GetPlayersFromServer());
    }

    private void Update()
    {
        if (canI)
        {
            UpdatePlayersLocation();
        }
    }


    private void GetPlayersLocationOnSpawn()
    {
        spawnedPlayers = new List<string>(playersLocation.Capacity);
        _locations = new List<Vector2d>(playersLocation.Count);
        for (var i = 0; i < playersLocation.Capacity; i++)
        {
            var locationString = playersLocation[i];
            _locations[i] = Conversions.StringToLatLon(locationString);
            var instance = Instantiate(_gameObject);
            instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i],true);
            _spawnedObject.Add(instance);
        }
        canI = true;
        
    }

    private void UpdatePlayersLocation()
    {
        var count = _spawnedObject.Count;
        for (var i = 0; i < count; i++)
        {
            if (_spawnedObject[i] == null)
            {
                continue;
            }

            var spawnedObject = _spawnedObject[i];
            var location = _locations[i];
            spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
            spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            
        }
    }

    private IEnumerator GetPlayersFromServer()
    {
        while (true)
        {
            using var www = new WWW(GameManager.BASE_URL + "get_all_players/");
            yield return www;
            var players = www.text.Split("|");
            
            foreach (var player in players)
            {
                player.Split(",");
                var temp = players[1] + ", " + players[2];
                playersLocation.Add(temp);
            }

            GetPlayersLocationOnSpawn();
            yield return new WaitForSeconds(3);
        }
    }

    
}
