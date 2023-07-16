using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Connections;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class
    GameManager : MonoBehaviour //TODO IF ANY PROBLEMS IN POLAYER INFO, USE GET PLAYER INFO SERVER METHOD, AND USE SETDATA METHOD FORM USER LOGIN

{
    public string BASE_URL = "http://robcops.de/";
    [HideInInspector] public string socialTab = "members/";
    public int amountOfClicks;

    public float clickPower;
    private AbstractMap _map;

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
    public int xp;
    public int currentRobUnionSafeID; //TODO make request for each when this is needed
    private bool firstLoadPlayers;
    public bool safe;

    private bool firstLoadSafes;
    //private List<string> locations = new List<string>();

    private Coroutine moneyRoutine;

    private SpawnOnMap spawnOnMap;

    public Coroutine updateOtherPlayersCoroutine;

    public Coroutine updateSafesCoroutine;

    public S_Error errorMessage;
    public S_Success successMessage;
    
    public float sfxVolume;
    public bool sfxOn;

    public static GameManager Instance { set; get; }

    private void Awake()
    {
        BASE_URL = "http://robcops.de/";
        currentRobUnionSafeID = -1;
        sfxOn = true;
        sfxVolume = 0.5f;
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        errorMessage = GameObject.Find("Error").GetComponent<S_Error>();

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
        if (scene.name == "RotationGame") Time.timeScale = 1;
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
            if (Instance.role == false) StartCoroutine(checkDistanceToSafes());
            _map = GameObject.FindWithTag("map").GetComponent<AbstractMap>();
           
        }
        else
        {
            if (moneyRoutine != null)
                StopCoroutine(moneyRoutine);
        }

        if (GameObject.Find("Error") != null)
        {
            errorMessage = GameObject.Find("Error").GetComponent<S_Error>();
        }
        if (GameObject.Find("Success") != null)
        {
            successMessage = GameObject.Find("Success").GetComponent<S_Success>();
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
            yield return new WaitForSeconds(30f);
        }
    }

    public IEnumerator checkDistanceToSafes()
    {
        yield return new WaitForSeconds(1.5f);
        using var www = new WWW(Instance.BASE_URL + "are_safes_near_you/");
        yield return www;
        Debug.Log(www.text);
        if (www.text.Equals("False")) StartCoroutine(SpawnSafeIfNotAnyInRange());
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
            if (www.text == "")
            {
                yield return new WaitForSeconds(10f);
                continue;
            }

            var allPlayerStrings = www.text.Split("|");

            for (var i = 0; i < allPlayerStrings.Length; i++)
            {
                var playerTupel = allPlayerStrings[i].Split(";");
                var rotationFloats = playerTupel[4].Split(":");
                if (rotationFloats.Length < 4)
                {
                    spawnOnMap.otherPlayers.Add(new C_OtherPlayerInfo(int.Parse(playerTupel[0]),
                        bool.Parse(playerTupel[1]),
                        new Vector2d(double.Parse(playerTupel[2].Replace(".", ",")),
                            double.Parse(playerTupel[3].Replace(".", ","))), new Quaternion(0f,
                            0f, 0f, 0f)));
                }
                else
                {
                    spawnOnMap.otherPlayers.Add(new C_OtherPlayerInfo(int.Parse(playerTupel[0]),
                        bool.Parse(playerTupel[1]),
                        new Vector2d(double.Parse(playerTupel[2].Replace(".", ",")),
                            double.Parse(playerTupel[3].Replace(".", ","))), new Quaternion(
                            float.Parse(rotationFloats[0]),
                            float.Parse(rotationFloats[1]), float.Parse(rotationFloats[2]),
                            float.Parse(rotationFloats[3]))));
                }
            }

            spawnOnMap.SpawnOtherPlayers();

            if (firstLoadPlayers)
            {
                firstLoadPlayers = false;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                yield return new WaitForSeconds(15f);
            }
        }
    }


    public bool InitializeSafe(int level, int cost)
    {
        //TODO COMMENT OUT FOR DEMO
        if (!SpawnChecker.CheckIsFreePos(_map, ImmediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude.x.ToString(CultureInfo.InvariantCulture),
                ImmediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude.y.ToString(CultureInfo.InvariantCulture)))
        {
            //Debug.Log("Cannot Spawn Safe In InitializeSafe, go outside");
            return false;
        }
        StartCoroutine(getMoneyAndSetLevel(level, cost));
        return true;
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

        yield return UpdateSafesAfterPlacing();
        yield return new WaitForSeconds(20f);
        updateSafesCoroutine = StartCoroutine(UpdateSafes());
        GameObject.Find("UI").GetComponent<UIManager>().setSafeMoney();
    }

    public IEnumerator UpdateSafes()
    {
        while (true)
        {
            //Innefficient shit code, but works. 
            spawnOnMap._locationStrings = new List<string>();
            for (var i = spawnOnMap._spawnedObjects.Count - 1; i >= 0; i--) spawnOnMap._spawnedObjects[i].Destroy();
           
            for (var i = spawnOnMap.cubes.Count - 1; i >= 0; i--) spawnOnMap.cubes[i].Destroy();
           
            spawnOnMap._spawnedObjects = new List<GameObject>();
            spawnOnMap.cubes = new List<GameObject>();


            using var www = new WWW(BASE_URL + "get_all_safes/");
            yield return www;
            if (www.text == "")
            {
                yield return new WaitForSeconds(10f);
                continue;
            }

            var safesTupels = www.text.Split("|");
            var ids = new List<int>();
            var levels = new List<int>();
            var locations = new List<string>();
            var hps = new List<int>();
            var stats = new List<int>();
            var robUns = new List<int>();

            for (var i = 0; i < safesTupels.Length; i++)
            {
                var tupel = safesTupels[i].Split(",");
                ids.Add(int.Parse(tupel[0]));
                levels.Add(int.Parse(tupel[1]));
                hps.Add(int.Parse(tupel[2]));
                locations.Add(tupel[3] + "," + tupel[4]);
                stats.Add(int.Parse(tupel[5]));
                robUns.Add(int.Parse(tupel[6]));
            }

            spawnOnMap.ids = ids;
            var idString = "";
            foreach (var id in spawnOnMap.ids) idString += id + ",";
          
            spawnOnMap.levels = levels;
            spawnOnMap.hps = hps;
            spawnOnMap._locationStrings = locations;
            spawnOnMap.statuses = stats;
            spawnOnMap.isRobunionList = robUns;
            spawnOnMap.SpawnCubes();
            spawnOnMap.WaitForCubeLocationThenSpawnSafe();
            if (firstLoadSafes)
            {
                firstLoadSafes = false;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                yield return new WaitForSeconds(20f);
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
        var robUns = new List<int>();
        for (var i = 0; i < safesTupels.Length; i++)
        {
            var tupel = safesTupels[i].Split(",");
            ids.Add(int.Parse(tupel[0]));
            levels.Add(int.Parse(tupel[1]));
            hps.Add(int.Parse(tupel[2]));
            locations.Add(tupel[3] + "," + tupel[4]);
            stats.Add(int.Parse(tupel[5]));
            robUns.Add(int.Parse(tupel[6]));
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
        spawnOnMap.isRobunionList = robUns;
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
                hp = 400;
                break;
            case 2:
                hp = 5000;
                break;
            case 3:
                hp = 50000;
                break;
            case 4:
                hp = 250000;
                break;
        }

        StartCoroutine(SendSafeToServer());
    }

    private IEnumerator SpawnSafeIfNotAnyInRange()
    {
        StopCoroutine(updateSafesCoroutine);
        using var www = new WWW(BASE_URL + "generate_random_safe/");
        yield return www;
        var text = www.text;
        var temp = text.Split("|");
        var locations = new List<string>();
        var textFinal = temp[0] + "," + temp[1];
        locations.Add(textFinal);
        yield return UpdateSafesAfterPlacing();
        yield return new WaitForSeconds(20f);
        updateSafesCoroutine = StartCoroutine(UpdateSafes());
    }

    public static void SetData(List<string> list)
    {
        Instance.username = list[1];
        Instance.money = int.Parse(list[2]);
        Instance.amountOfClicks = int.Parse(list[3]);
        Instance.clickPower = float.Parse(list[4]);
        Instance.location = new Vector2(float.Parse(list[5]), float.Parse(list[6]));
        Instance.role = bool.Parse(list[7]);
        int guildID;
        if (!Instance.role && int.TryParse(list[8], out guildID))
        {
            Instance.guild = guildID;
        }
        else if (int.TryParse(list[9], out guildID))
            Instance.guild = guildID;
        else
            Instance.guild = -1;
    
        Instance.userId = int.Parse(list[10]);

        int currentXP;
        if (!Instance.role && int.TryParse(list[12], out currentXP))
        {
            Instance.xp = currentXP;
        }
        else if (int.TryParse(list[11], out currentXP))
            Instance.xp = currentXP;
        else
            Instance.xp = 0;
    }

    public IEnumerator getPlayerInfo()
    {
        using var www = new WWW(Instance.BASE_URL + "get_player_info/");
        yield return www;
        List<string> data = S_Parser.ParseResponse(www.text);
        SetData(data);
    }

    public IEnumerator getPlayerInfoUpdate() //TODO USE IF THERE ARE UI BUGS
    {
        while (true)
        {
            using var www = new WWW(Instance.BASE_URL + "get_player_info/");
            yield return www;
            List<string> data = S_Parser.ParseResponse(www.text);
            SetData(data);
            yield return new WaitForSeconds(30f);
        }
        
    }
    
    public IEnumerator getRobUnionSafeID()
    {
        using var www = new WWW(Instance.BASE_URL + "get_robunion_safe_id/");
        yield return www;
        Instance.currentRobUnionSafeID = int.Parse(www.text);
    }
    
    public IEnumerator resetRobUnionSafeID()
    {
        using var www = new WWW(Instance.BASE_URL + "reset_robunion_safe_id/");
        yield return www;
        Instance.currentRobUnionSafeID = int.Parse(www.text);
    }
}