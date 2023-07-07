using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Mapbox.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string BASE_URL = "http://chernogop.pythonanywhere.com/";
    [HideInInspector] public string socialTab = "members/";
    public int amountOfClicks;

    public float clickPower;

    //public S_UserLogin client;
    public int guild;
    public int currentHP;
    public int hp;
    public int currentMinutes;
    public int currentSeconds;
    public ImmediatePositionWithLocationProvider ImmediatePositionWithLocationProvider;
    public int level;
    public Vector2 location;
    public int money;
    public bool role;
    public string username;

    private SpawnOnMap spawnOnMap;
    public static GameManager Instance { set; get; }

    private void Awake()
    {
        BASE_URL = "http://87.143.147.178:8000/";
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            ImmediatePositionWithLocationProvider =
                GameObject.FindWithTag("Player").GetComponent<ImmediatePositionWithLocationProvider>();
            //client = GameObject.FindWithTag("Server").GetComponent<S_UserLogin>();
            spawnOnMap = GameObject.FindWithTag("Spawner").GetComponent<SpawnOnMap>();
            Instance.GetAllSafes();
        }
    }


    public void GetAllSafes()
    {
        StartCoroutine(GetSafeInfo());
    }

    public void InitializeSafe(int level, int cost)
    {
        StartCoroutine(getMoneyAndSetLevel(level, cost));
    }

    public IEnumerator SendSafeToServer()
    {
        var locationX = ImmediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude.x.ToString(CultureInfo.InvariantCulture);
        // 48.264518.ToString().Replace(",",".");
        var locationY = ImmediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude.y.ToString(CultureInfo.InvariantCulture);
        //11.6713515.ToString().Replace(",", ".");

        var form = new WWWForm();
        form.AddField("level", level);
        form.AddField("hp", hp);
        form.AddField("locationX", locationX);
        form.AddField("locationY", locationY);

        using var www = new WWW(BASE_URL + "place_safe/", form);
        yield return www;
        Debug.Log(www.text);
        //StartCoroutine(GetSafeInfo()); //todo fix so that only one safe is added, and not all safes are fetched again
    }

    public IEnumerator GetSafeInfo()
    {
        using var www = new WWW(BASE_URL + "get_all_safes/");
        yield return www;
        Debug.Log(www.text);
        var safesTupels = www.text.Split("|");
        var ids = new List<int>();
        var levels = new List<int>();
        var hps = new List<int>();
        var locations = new List<string>();
        for (var i = 1; i < safesTupels.Length; i++)
        {
            var tupel = safesTupels[i].Split(",");
            ids.Add(int.Parse(tupel[0]));
            levels.Add(int.Parse(tupel[1]));
            hps.Add(int.Parse(tupel[2]));
            locations.Add(tupel[3] + "," + tupel[4]);
            Debug.Log(tupel[3] + "," + tupel[4]);
            Debug.Log(int.Parse(tupel[0]));
        }

        spawnOnMap.ids = ids;
        spawnOnMap.levels = levels;
        spawnOnMap.hps = hps;
        spawnOnMap._locationStrings = locations;
        spawnOnMap.SpawnCubes();
        spawnOnMap.waitForCubeLocationThenSpawnSafe();
    }

    public IEnumerator getMoneyAndSetLevel(int level, int cost)
    {
        var form = new WWWForm();
        form.AddField("cost", cost);
        using var www = new WWW(BASE_URL + "pay_money/", form);
        yield return www;
        Debug.Log(www.text);
        money = int.Parse(www.text);
        this.level = level;
        switch (level)
        {
            case 1:
                hp = 50000;
                break;
            case 2:
                hp = 100000;
                break;
            case 3:
                hp = 250000;
                break;
            case 4:
                hp = 500000;
                break;
        }

        StartCoroutine(SendSafeToServer());
    }
}