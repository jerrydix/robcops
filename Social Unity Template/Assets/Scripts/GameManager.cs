using System.Collections;
using System.Collections.Generic;
using Mapbox.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public string BASE_URL = "http://127.0.0.1:8000/";
    [HideInInspector] public string socialTab = "members/";
    public int amountOfClicks;
    public float clickPower;
    public S_UserLogin client;
    public Guild guild; //todo fetch guilds from server before login, save them in eg. game manager
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
            client = GameObject.FindWithTag("Server").GetComponent<S_UserLogin>();
            spawnOnMap = GameObject.FindWithTag("Spawner").GetComponent<SpawnOnMap>();
            Instance.GetAllSafes();
        }
    }


    public void GetAllSafes()
    {
        StartCoroutine(GetSafeInfo());
    }

    public void InitializeSafe()
    {
        StartCoroutine(getMoneyAndSetLevel());
    }

    public IEnumerator SendSafeToServer()
    {
        var locationX =
            48.264518.ToString()
                .Replace(",",
                    "."); //ImmediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude.x.ToString(CultureInfo.InvariantCulture);
        var locationY =
            11.6713515.ToString()
                .Replace(",",
                    "."); //ImmediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude.y.ToString(CultureInfo.InvariantCulture);

        var form = new WWWForm();
        form.AddField("level", level);
        form.AddField("hp", hp);
        form.AddField("locationX", locationX);
        form.AddField("locationY", locationY);
        Debug.Log(client);
        using var www = new WWW(client.BASE_URL + "create_safe/", form);
        yield return www;
        Debug.Log(www.text);
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
            Debug.Log(tupel.Length);
            Debug.Log(safesTupels.Length);
            ids.Add(int.Parse(tupel[0]));
            levels.Add(int.Parse(tupel[1]));
            hps.Add(int.Parse(tupel[2]));
            locations.Add(tupel[3] + "," + tupel[4]);
            Debug.Log(tupel[3] + "," + tupel[4]);
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
        using var www = new WWW(BASE_URL + "get_money/");
        yield return www;
        var temp = www.text;
        Debug.Log(temp);
        money = int.Parse(temp);
        if ((money < 2000) & (money > 100))
            level = 1;
        else if (money < 5000 && money > 2000)
            level = 2;
        else if (money < 10000 && money > 5000)
            level = 3;
        else if (money < 20000 && money > 10000) level = 4;

        hp = 1000 * level;
        StartCoroutine(SendSafeToServer());
    }
}