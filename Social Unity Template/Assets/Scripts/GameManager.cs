using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Mapbox.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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
    private int level;
    private int hp;
    private S_UserLogin client;
    private ImmediatePositionWithLocationProvider ImmediatePositionWithLocationProvider;

    private SpawnOnMap spawnOnMap;
    public static GameManager Instance { set; get; }
    /*private GameManager()
    {
        if (instance != null)
            return;
        instance = this;
    }
    
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }*/

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            ImmediatePositionWithLocationProvider =
                GameObject.FindWithTag("Player").GetComponent<ImmediatePositionWithLocationProvider>();
            client = GameObject.FindWithTag("Server").GetComponent<S_UserLogin>();
            spawnOnMap = GameObject.FindWithTag("Spawner").GetComponent<SpawnOnMap>();
        }
        
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void GetAllSafes()
    {
        StartCoroutine(getSafeInfo());
    }

    public IEnumerator SendSafeToServer()
    {
        string locationX = 48.264518.ToString().Replace(",", ".");//ImmediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude.x.ToString(CultureInfo.InvariantCulture);
        string locationY = 11.6713515.ToString().Replace(",", ".");//ImmediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude.y.ToString(CultureInfo.InvariantCulture);
        
        WWWForm form = new WWWForm();
        form.AddField("level", level);
        form.AddField("hp", hp);
        form.AddField("locationX", locationX);
        form.AddField("locationY", locationY);
        Debug.Log(client);
        using WWW www = new WWW(client.BASE_URL + "create_safe/", form);
        yield return www;
        Debug.Log(www.text);

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
    
    public IEnumerator getMoneyAndSetLevel()
    {
        using WWW www = new WWW(BASE_URL + "get_money/");
        yield return www;
        string temp = www.text;
        Debug.Log(temp);
        money = int.Parse(temp);
        if (money < 2000 & money > 100)
        {
            level = 1;
            
        } 
        else if (money < 5000 && money > 2000)
        {
            level = 2;
            
        }
        else if (money < 10000 && money > 5000)
        {
            level = 3;
            
        }
        else if(money < 20000 && money > 10000)
        {
            level = 4;
        }

        hp = 1000 * level;
        StartCoroutine(SendSafeToServer());
    }
}
