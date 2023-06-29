using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Examples;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public string BASE_URL = "http://127.0.0.1:8000/";
    [HideInInspector] public string socialTab = "members/";
    private string username;
    private int money;
    private bool role;
    private int amountOfClicks;
    private float clickPower;
    private Vector2 location = new Vector2();
    private Guild guild; //todo fetch guilds from server before login, save them in eg. game manager

    private SpawnOnMap spawnOnMap;


    private void Start()
    {
        spawnOnMap = GameObject.FindWithTag("Spawner").GetComponent<SpawnOnMap>();
    }

    public void GetAllSafes()
    {
        StartCoroutine(getSafeInfo());
    }
    
    public IEnumerator getSafeInfo()
    {
        using WWW www = new WWW(BASE_URL + "get_all_safes/");
        yield return www;
        Debug.Log(www.text);
        var safesTupels = www.text.Split("|");
        List<int> ids = new List<int>();
        List<int> levels = new List<int>();
        List<int> hps = new List<int>();
        List<string> locations = new List<string>();
        for (int i = 1; i < safesTupels.Length; i++)
        {
            var tupel = safesTupels[i].Split(",");
            Debug.Log(tupel.Length);
            Debug.Log(safesTupels.Length);
            ids.Add( int.Parse(tupel[0]));
            levels.Add(int.Parse(tupel[1]));
            hps.Add(int.Parse(tupel[2]));
            locations.Add(tupel[3].ToString() + "," + tupel[4].ToString());
            Debug.Log(tupel[3].ToString() + "," + tupel[4].ToString());
        }
        
        spawnOnMap.ids = ids;
        spawnOnMap.levels = levels;
        spawnOnMap.hps = hps;
        spawnOnMap._locationStrings = locations;
        spawnOnMap.SpawnCubes();
        spawnOnMap.waitForCubeLocationThenSpawnSafe();
    }
}
