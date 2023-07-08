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

    public Coroutine updateSafesCoroutine;
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
            //Instance.GetAllSafes();
            updateSafesCoroutine = StartCoroutine(UpdateSafes());
        }
    }


    /*public void GetAllSafes()
    {
        StartCoroutine(GetSafeInfo());
    }*/

    public void InitializeSafe(int level, int cost)
    {
        StartCoroutine(getMoneyAndSetLevel(level, cost));
    }

    public IEnumerator SendSafeToServer()
    {
        var locationX =
            ImmediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude.x.ToString(
                CultureInfo.InvariantCulture);
        // 48.264518.ToString().Replace(",",".");
        var locationY =
            ImmediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude.y.ToString(
                CultureInfo.InvariantCulture);
        //11.6713515.ToString().Replace(",", ".");

        var form = new WWWForm();
        form.AddField("level", level);
        form.AddField("hp", hp);
        form.AddField("locationX", locationX);
        form.AddField("locationY", locationY);

        using var www = new WWW(BASE_URL + "place_safe/", form);
        yield return www;
        Debug.Log(www.text);

        yield return UpdateSafesAfterPlacing();
        yield return new WaitForSeconds(60f);
        updateSafesCoroutine = StartCoroutine(UpdateSafes());

        //StartCoroutine(GetSafeInfo()); //todo fix so that only one safe is added, and not all safes are fetched again
    }

    /*public IEnumerator GetSafeInfo()
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
            //Debug.Log(tupel[3] + "," + tupel[4]);
            //Debug.Log(int.Parse(tupel[0]));
        }

        spawnOnMap.ids = ids;
        spawnOnMap.levels = levels;
        spawnOnMap.hps = hps;
        spawnOnMap._locationStrings = locations;
        spawnOnMap.SpawnCubes();
        //yield return StartCoroutine(spawnOnMap.WaitForCubeLocationThenSpawnSafe());
        spawnOnMap.WaitForCubeLocationThenSpawnSafe();
    }*/

    public IEnumerator UpdateSafes()
    {
        while (true)
        {
            //Innefficient shit code, but works. 
            spawnOnMap._locationStrings = new List<string>();
            for (var i = spawnOnMap._spawnedObjects.Count - 1; i >= 0; i--)
            {
                spawnOnMap._spawnedObjects[i].Destroy();
               // spawnOnMap._spawnedObjects.RemoveAt(i);
            }

            for (var i = spawnOnMap.cubes.Count - 1; i >= 0; i--)
            {
                spawnOnMap.cubes[i].Destroy();
                //spawnOnMap.cubes.RemoveAt(i);
            }

            spawnOnMap._spawnedObjects = new List<GameObject>();
            spawnOnMap.cubes = new List<GameObject>();


            using var www = new WWW(BASE_URL + "get_all_safes/");
            yield return www;
            Debug.Log("getAllSafesText: " + www.text);
            if (www.text == "")
            {
                yield return new WaitForSeconds(10f);
                continue;
            }
            var safesTupels = www.text.Split("|");
            var ids = new List<int>();
            var levels = new List<int>();
            var hps = new List<int>();
            var locations = new List<string>();
            for (var i = 0; i < safesTupels.Length; i++)
            {
                var tupel = safesTupels[i].Split(",");
                ids.Add(int.Parse(tupel[0]));
                levels.Add(int.Parse(tupel[1]));
                hps.Add(int.Parse(tupel[2]));
                locations.Add(tupel[3] + "," + tupel[4]);
                //Debug.Log(tupel[3] + "," + tupel[4]);
                //Debug.Log(int.Parse(tupel[0]));
            }

            spawnOnMap.ids = ids;
            var idString = "";
            foreach (var id in spawnOnMap.ids)
            {
                idString += id + ",";
            }
            Debug.Log("idstring: " + idString);
            spawnOnMap.levels = levels;
            spawnOnMap.hps = hps;
            spawnOnMap._locationStrings = locations;
            spawnOnMap.SpawnCubes();
            spawnOnMap.WaitForCubeLocationThenSpawnSafe();
            yield return new WaitForSeconds(60f);
        }
    }
    
    public IEnumerator UpdateSafesAfterPlacing()
    {
        
        //Innefficient shit code, but works. 
        spawnOnMap._locationStrings = new List<string>();
        for (var i = spawnOnMap._spawnedObjects.Count - 1; i >= 0; i--)
        {
            spawnOnMap._spawnedObjects[i].Destroy();
           // spawnOnMap._spawnedObjects.RemoveAt(i);
        }

        for (var i = spawnOnMap.cubes.Count - 1; i >= 0; i--)
        {
            spawnOnMap.cubes[i].Destroy();
            //spawnOnMap.cubes.RemoveAt(i);
        }

        spawnOnMap._spawnedObjects = new List<GameObject>();
        spawnOnMap.cubes = new List<GameObject>();


        using var www = new WWW(BASE_URL + "get_all_safes/");
        yield return www;
        Debug.Log("getAllSafesText: " + www.text);
        if (www.text == "")
        {
            yield return new WaitForSeconds(3f);
        }
        var safesTupels = www.text.Split("|");
        var ids = new List<int>();
        var levels = new List<int>();
        var hps = new List<int>();
        var locations = new List<string>();
        for (var i = 0; i < safesTupels.Length; i++)
        {
            var tupel = safesTupels[i].Split(",");
            ids.Add(int.Parse(tupel[0]));
            levels.Add(int.Parse(tupel[1]));
            hps.Add(int.Parse(tupel[2]));
            locations.Add(tupel[3] + "," + tupel[4]);
            //Debug.Log(tupel[3] + "," + tupel[4]);
            //Debug.Log(int.Parse(tupel[0]));
        }

        spawnOnMap.ids = ids;
        var idString = "";
        foreach (var id in spawnOnMap.ids)
        {
            idString += id + ",";
        }
        Debug.Log("idstring: " + idString);
        spawnOnMap.levels = levels;
        spawnOnMap.hps = hps;
        spawnOnMap._locationStrings = locations;
        spawnOnMap.SpawnCubes();
        spawnOnMap.WaitForCubeLocationThenSpawnSafe();
    }

    public IEnumerator getMoneyAndSetLevel(int level, int cost)
    {
        StopCoroutine(updateSafesCoroutine);
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