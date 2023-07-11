using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Mapbox.Examples;
using Mathd = Mapbox.Utils.Mathd;
using Vector2d = Mapbox.Utils.Vector2d;

using Mapbox.Utils;

using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public string BASE_URL = "http://87.143.147.178:8000/";
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
    public int userId;
    private bool firstLoadPlayers;
    private bool firstLoadSafes;
    //private List<string> locations = new List<string>();

    private Coroutine moneyRoutine;

    private SpawnOnMap spawnOnMap;

    public Coroutine updateSafesCoroutine;

    public Coroutine updateOtherPlayersCoroutine;

    //private bool cannotPlaceAnyMoreSafes = false;

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
            firstLoadSafes = true;
            firstLoadPlayers = true;
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
            
            for (var i = spawnOnMap.playersObjects.Count - 1; i >= 0; i--) spawnOnMap.playersObjects[i].Destroy();

            spawnOnMap.otherPlayers = new List<C_OtherPlayerInfo>();

            using var www = new WWW(BASE_URL + "get_all_locations/");
            yield return www;
            //Debug.Log("getAllLocations: " + www.text);
            if (www.text == "")
            {
                yield return new WaitForSeconds(10f);
                continue;
            }

            string[] allPlayerStrings = www.text.Split("|");
            
            for (var i = 0; i < allPlayerStrings.Length; i++)
            {
                string[] playerTupel = allPlayerStrings[i].Split(";");
                string[] rotationFloats = playerTupel[4].Split(":");
                if (rotationFloats.Length < 4)
                {
                    spawnOnMap.otherPlayers.Add(new C_OtherPlayerInfo(int.Parse(playerTupel[0]), bool.Parse(playerTupel[1]), new Vector2d(double.Parse(playerTupel[2]), double.Parse(playerTupel[3])), new Quaternion(0f,
                        0f, 0f, 0f)));
                }
                else
                {
                    foreach (var rot in rotationFloats)
                    {
                        //Debug.Log("Length" + rotationFloats.Length);
                        //Debug.Log(rot);
                    }
                    spawnOnMap.otherPlayers.Add(new C_OtherPlayerInfo(int.Parse(playerTupel[0]), bool.Parse(playerTupel[1]), new Vector2d(double.Parse(playerTupel[2]), double.Parse(playerTupel[3])), new Quaternion(float.Parse(rotationFloats[0]),
                        float.Parse(rotationFloats[1]), float.Parse(rotationFloats[2]), float.Parse(rotationFloats[3])))); 
                }
            }
            
            spawnOnMap.SpawnOtherPlayers();
            
            if (firstLoadPlayers)
            {
                firstLoadPlayers = false;
                yield return new WaitForSeconds(0.1f); //TODO FIX THIS
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
                /*if (!cannotPlaceAnyMoreSafes)
                {
                    //CreateSafeIfNone();   
                }*/
                yield return new WaitForSeconds(10f);
                continue;
            }

            var safesTupels = www.text.Split("|");
            var ids = new List<int>();
            var levels = new List<int>();
            var locations = new List<string>();
            var hps = new List<int>();
            var stats = new List<int>();
            for (var i = 0; i < safesTupels.Length; i++)
            {
                var tupel = safesTupels[i].Split(",");
                ids.Add(int.Parse(tupel[0]));
                levels.Add(int.Parse(tupel[1]));
                hps.Add(int.Parse(tupel[2]));
                locations.Add(tupel[3] + "," + tupel[4]);
                stats.Add(int.Parse(tupel[5]));
                //Debug.Log(tupel[3] + "," + tupel[4]);
                //Debug.Log(int.Parse(tupel[0]));
            }

            spawnOnMap.ids = ids;
            var idString = "";
            foreach (var id in spawnOnMap.ids) idString += id + ",";
            //Debug.Log("idstring: " + idString);
            spawnOnMap.levels = levels;
            spawnOnMap.hps = hps;
            spawnOnMap._locationStrings = locations;
            spawnOnMap.statuses = stats;
            spawnOnMap.SpawnCubes();
            spawnOnMap.WaitForCubeLocationThenSpawnSafe();
            if (firstLoadSafes)
            {
                firstLoadSafes = false;
                yield return new WaitForSeconds(0.1f); //TODO FIX THIS
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
       // Debug.Log("getAllSafesText: " + www.text);
        if (www.text == "") yield return new WaitForSeconds(3f);
        var safesTupels = www.text.Split("|");
        var ids = new List<int>();
        var levels = new List<int>();
        var hps = new List<int>();
        var locations = new List<string>();
        var stats = new List<int>();
        for (var i = 0; i < safesTupels.Length; i++)
        {
            var tupel = safesTupels[i].Split(",");
            ids.Add(int.Parse(tupel[0]));
            levels.Add(int.Parse(tupel[1]));
            hps.Add(int.Parse(tupel[2]));
            locations.Add(tupel[3] + "," + tupel[4]);
            stats.Add(int.Parse(tupel[5]));
            //Debug.Log(tupel[3] + "," + tupel[4]);
            //Debug.Log(int.Parse(tupel[0]));
        }

        spawnOnMap.ids = ids;
        var idString = "";
        foreach (var id in spawnOnMap.ids) idString += id + ",";
        //Debug.Log("idstring: " + idString);
        spawnOnMap.levels = levels;
        spawnOnMap.hps = hps;
        spawnOnMap._locationStrings = locations;
        spawnOnMap.statuses = stats;
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
        //Debug.Log(www.text);
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

    /*private void CreateSafeIfNone()
    {
        //Check if there any safes are in Range
        for (int i = 0; i < locations.Count; i++)
        {
            var LocationFromList = Conversions.StringToLatLon(locations[i]);
            if (CalculateDistanceToNorm(LocationFromList) || CalculateDistanceInEditor(LocationFromList))
            {
                return;
            }
        }

        //Randomly Generate Safe and Location
        var position = new Vector2d(48.264518, 11.6713515);
        //var position = ImmediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude;
        var num = Random.Range(1.0f,4.0f);
        Debug.Log(num);
        

        for (var i = 0; i < num; i++)
        {
            var deviations = Random.Range(0.1f,0.6f);
            var deviations1 = Random.Range(0.1f,0.6f);
            var NewPosition = new Vector2d(position.x + deviations, position.y + deviations1);
            var Level = GetRandomLevel();
            var Health = GetCorrespondingNumber(Level);
            //StartCoroutine(CreateSafe(NewPosition.x.ToString(), NewPosition.y.ToString(), Health, Level));
        }
    }*/

    private int GetCorrespondingNumber(int level)
    {
        switch (level)
        {
            case 1:
            {
                return 50000;
            }
            case 2:
            {
                return 100000;
            }
            case 3:
            {
                return 250000;
            }
            case 4:
            {
                return 500000;
            }
            default: return 1;
        }
    }

    private int GetRandomLevel()
    {
        var num = Random.Range(0f, 4f);
        switch (num)
        {
            case 1:
            {
                return 1;
            }
            case 2:
            {
                return 2;
            }
            case 3:
            {
                return 3;
            }
            case 4:
            {
                return 4;
            }
            default: return 1;
        }
    }

    /*private IEnumerator CreateSafe(string locationX, string locationY, int hp, int level)
    {
        var form = new WWWForm();
        form.AddField("level", level);
        form.AddField("hp", hp);
        form.AddField("locationX", locationX);
        form.AddField("locationY", locationY);
        using var www = new WWW(BASE_URL + "create_safe/", form);
        yield return www;
        yield return UpdateSafes();
        yield return new WaitForSeconds(60f);
        yield return UpdateSafesAfterPlacing();
    }*/

    
    /*private IEnumerator SendNewSafesToServer(string locationX, string locationY, int Health, int Level)
    {
        var form = new WWWForm();
        form.AddField("level", Level);
        form.AddField("hp", Health);
        form.AddField("locationX", locationX);
        form.AddField("locationY", locationY);
        using var www = new WWW(BASE_URL + "place_safe/", form);
        yield return www;
        Debug.Log(www.text);

        yield return UpdateSafesAfterPlacing();
        yield return new WaitForSeconds(60f);
        updateSafesCoroutine = StartCoroutine(UpdateSafes());
    }*/


    private bool CalculateDistanceToNorm(Vector2d location)
    {
        for (var i = 0; i < spawnOnMap._locationStrings.Count; i++)
        {
            if (spawnOnMap._locationStrings[i] == null) continue;
            //Get Locations of Safe and Player

            //var currentString = spawnOnMap._locationStrings[i];
            var instance = location;
            var x = Conversions.StringToLatLon(ImmediatePositionWithLocationProvider.LocationProvider
                .CurrentLocation.LatitudeLongitude.ToString());
            var playerLocation = x.x;
            var playerLocationy = x.y;

            //Calculate the Distance

            var deltaLat = (instance.x - playerLocation) * Mathd.PI / 180;
            var deltaLon = (instance.y - playerLocationy) * Mathd.PI / 180;

            var calc = Mathd.Pow(Mathd.Sin(deltaLat / 2), 2) + Mathd.Cos(playerLocation * Mathd.PI / 180)
                * Mathd.Cos(instance.x * Mathd.PI / 180) * Mathd.Pow(Mathd.Sin(deltaLon / 2), 2);
            var temp = 2 * Mathd.Atan2(Mathd.Sqrt(calc), Mathd.Sqrt(1 - calc));
            var result = 6371 * temp;
            result *= 1000;
            var finalResult = Mathd.Abs(result);

            //Filter Safes that are more than 1km away

            if (finalResult <= 350) return true;
        }

        return false;
    }


    private bool CalculateDistanceInEditor(Vector2d location)
    {
        for (var i = 0; i < spawnOnMap._locationStrings.Count; i++)
        {
            if (spawnOnMap._locationStrings[i] == null) continue;
            //Get Locations of Safe and Player

            //var currentString = spawnOnMap._locationStrings[i];
            var instance = location;
            var x = new Vector2d(48.264518, 11.6713515);
            var playerLocation = x.x;
            var playerLocationy = x.y;

            //Calculate the Distance

            var deltaLat = (instance.x - playerLocation) * Mathd.PI / 180;
            var deltaLon = (instance.y - playerLocationy) * Mathd.PI / 180;

            var calc = Mathd.Pow(Mathd.Sin(deltaLat / 2), 2) + Mathd.Cos(playerLocation * Mathd.PI / 180)
                * Mathd.Cos(instance.x * Mathd.PI / 180) * Mathd.Pow(Mathd.Sin(deltaLon / 2), 2);
            var temp = 2 * Mathd.Atan2(Mathd.Sqrt(calc), Mathd.Sqrt(1 - calc));
            var result = 6371 * temp;
            result *= 1000;
            var finalResult = Mathd.Abs(result);

            //Filter Safes that are more than 1km away

            if (finalResult <= 350) return true;
        }

        return false;
    }
}