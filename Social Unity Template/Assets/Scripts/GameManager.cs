using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Mapbox.Examples;
using Mapbox.Utils;
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
    public int currentSafeLevel;
    public ImmediatePositionWithLocationProvider ImmediatePositionWithLocationProvider;
    public int level;
    public Vector2 location;
    public int money;
    public bool role;
    public string username;
    private bool firstLoad;

    private Coroutine moneyRoutine;

    private SpawnOnMap spawnOnMap;

    public Coroutine updateSafesCoroutine;
    public Coroutine updateOtherPlayersCoroutine;
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
            firstLoad = true;
            moneyRoutine = StartCoroutine(GetPlayerMoney());
            ImmediatePositionWithLocationProvider =
                GameObject.FindWithTag("Player").GetComponent<ImmediatePositionWithLocationProvider>();
            spawnOnMap = GameObject.FindWithTag("Spawner").GetComponent<SpawnOnMap>();
            updateSafesCoroutine = StartCoroutine(UpdateSafes());
            updateOtherPlayersCoroutine = StartCoroutine(UpdateOtherPlayers());
        }
        else
        {
            if (moneyRoutine != null)
                StopCoroutine(moneyRoutine);
        }
    }

    public IEnumerator GetPlayerMoney()
    {
        while (true)
        {
            using var www = new WWW(Instance.BASE_URL + "get_money/");
            yield return www;
            Debug.Log(www.text);
            var money = int.Parse(www.text);
            Instance.money = money;
            yield return new WaitForSeconds(60f);
        }
    }

    public IEnumerator GetPlayerMoneyOnce()
    {
        using var www = new WWW(Instance.BASE_URL + "get_money/");
        yield return www;
        Debug.Log(www.text);
        var money = int.Parse(www.text);
        Instance.money = money;
    }
    
    public IEnumerator UpdateOtherPlayers()
    {
        while (true)
        {
            //Innefficient shit code, but works. 
            
            for (var i = spawnOnMap.otherPlayers.Count - 1; i >= 0; i--) spawnOnMap.otherPlayers[i].Destroy();

            spawnOnMap.otherPlayers = new List<OtherPlayer>();
            
            using var www = new WWW(BASE_URL + "get_all_locations/");
            yield return www;
            Debug.Log("getAllLocations: " + www.text);
            if (www.text == "")
            {
                yield return new WaitForSeconds(10f);
                continue;
            }

            string[] allPlayerStrings = www.text.Split("|");
            
            for (var i = 0; i < allPlayerStrings.Length; i++)
            {
                Debug.Log(allPlayerStrings[i]);
                string[] playerTupel = allPlayerStrings[i].Split(";");
                spawnOnMap.otherPlayers[i].id = int.Parse(playerTupel[0]);
                spawnOnMap.otherPlayers[i].role = bool.Parse(playerTupel[1]);
                spawnOnMap.otherPlayers[i].location = new Vector2d(double.Parse(playerTupel[2]), double.Parse(playerTupel[3]));
                string[] rotationFloats = playerTupel[4].Split(",");

                spawnOnMap.otherPlayers[i].rotation = new Quaternion(float.Parse(rotationFloats[0]),
                    float.Parse(rotationFloats[1]), float.Parse(rotationFloats[2]), float.Parse(rotationFloats[3]));
                
            }
            
            spawnOnMap.SpawnOtherPlayers();
            
            if (firstLoad)
            {
                yield return new WaitForSeconds(0.5f); //TODO FIX THIS
                firstLoad = false;
            }
            else
            {
                yield return new WaitForSeconds(15f);
            }
        }
    }


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
    }

    public IEnumerator UpdateSafes()
    {
        while (true)
        {
            //Innefficient shit code, but works. 
            spawnOnMap._locationStrings = new List<string>();
            for (var i = spawnOnMap._spawnedObjects.Count - 1; i >= 0; i--) spawnOnMap._spawnedObjects[i].Destroy();
            // spawnOnMap._spawnedObjects.RemoveAt(i);
            for (var i = spawnOnMap.cubes.Count - 1; i >= 0; i--) spawnOnMap.cubes[i].Destroy();
            //spawnOnMap.cubes.RemoveAt(i);
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
            foreach (var id in spawnOnMap.ids) idString += id + ",";
            Debug.Log("idstring: " + idString);
            spawnOnMap.levels = levels;
            spawnOnMap.hps = hps;
            spawnOnMap._locationStrings = locations;
            spawnOnMap.SpawnCubes();
            spawnOnMap.WaitForCubeLocationThenSpawnSafe();
            if (firstLoad)
            {
                yield return new WaitForSeconds(0.5f); //TODO FIX THIS
                firstLoad = false;
            }
            else
            {
                yield return new WaitForSeconds(60f);
            }
        }
    }

    public IEnumerator UpdateSafesAfterPlacing()
    {
        //Innefficient shit code, but works. 
        spawnOnMap._locationStrings = new List<string>();
        for (var i = spawnOnMap._spawnedObjects.Count - 1; i >= 0; i--) spawnOnMap._spawnedObjects[i].Destroy();
        // spawnOnMap._spawnedObjects.RemoveAt(i);
        for (var i = spawnOnMap.cubes.Count - 1; i >= 0; i--) spawnOnMap.cubes[i].Destroy();
        //spawnOnMap.cubes.RemoveAt(i);
        spawnOnMap._spawnedObjects = new List<GameObject>();
        spawnOnMap.cubes = new List<GameObject>();


        using var www = new WWW(BASE_URL + "get_all_safes/");
        yield return www;
        Debug.Log("getAllSafesText: " + www.text);
        if (www.text == "") yield return new WaitForSeconds(3f);
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
        foreach (var id in spawnOnMap.ids) idString += id + ",";
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