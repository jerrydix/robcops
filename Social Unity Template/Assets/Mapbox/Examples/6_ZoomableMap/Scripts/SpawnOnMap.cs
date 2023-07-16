using System.Collections.Generic;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = Unity.Mathematics.Random;

namespace Mapbox.Examples
{
    public class SpawnOnMap : MonoBehaviour
    {
        private static bool activateUpdate;
        [SerializeField] private AbstractMap _map;

        [SerializeField] [Geocode] public List<string> _locationStrings;

        [SerializeField] private float _spawnScale = 100f;


        public GameObject _markerPrefab;

        public List<GameObject> _spawnedObjects;
        public List<GameObject> cubes = new();
        [SerializeField] private GameObject testo;
        public List<int> ids = new();
        public List<int> levels = new();
        public List<int> hps = new();
        public List<int> statuses = new();
        public List<int> isRobunionList = new();
        public Vector2d[] _cubeLocations;
        public Vector2d[] _locations;
        private ImmediatePositionWithLocationProvider _immediatePositionWithLocationProvider;
        private LocationArrayEditorLocationProvider _locationArrayEditorLocationProvider;
        
        public List<C_OtherPlayerInfo> otherPlayers;
        public List<GameObject> playersObjects;

        private SafeSpinScript SafeSpinScript;
        
        [SerializeField] private GameObject robberPrefab;
        [SerializeField] private GameObject copPrefab;

        private void Start()
        {
            //CreateSafeIfNoneAreInRange();
            _locationArrayEditorLocationProvider = GameObject.FindWithTag("EditorOnly")
                .GetComponent<LocationArrayEditorLocationProvider>();
            _immediatePositionWithLocationProvider = GameObject.FindWithTag("Player")
                .GetComponent<ImmediatePositionWithLocationProvider>();
            otherPlayers = new List<C_OtherPlayerInfo>();
            playersObjects = new List<GameObject>();
        }

        public void SpawnOtherPlayers()
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                foreach (var otherPlayer in otherPlayers)
                {
                    if(otherPlayer.id == GameManager.Instance.userId)
                        continue;
                    GameObject instance;
                    if (otherPlayer.role)
                    {
                        instance = Instantiate(copPrefab);
                    }
                    else
                    {
                        instance = Instantiate(robberPrefab);
                    }
                    Debug.Log(" ");
                    Debug.Log(otherPlayer.location.ToString());
                    instance.transform.localPosition = _map.GeoToWorldPosition(otherPlayer.location);
                    instance.transform.localRotation = otherPlayer.rotation;
                    //Debug.Log("INSTANCE"+ instance.GetComponent<OtherPlayer>());
                    instance.GetComponent<OtherPlayer>().role = otherPlayer.role;
                    instance.GetComponent<OtherPlayer>().id = otherPlayer.id;
                    instance.GetComponent<OtherPlayer>().location = otherPlayer.location;
                    instance.GetComponent<OtherPlayer>().rotation = otherPlayer.rotation;
                    playersObjects.Add(instance);
                }       
            }
        } 

        public void WaitForCubeLocationThenSpawnSafe()
        {
            _locations = new Vector2d[_locationStrings.Count];
            _spawnedObjects = new List<GameObject>();
            for (var i = 0; i < _locationStrings.Count; i++)
            {
                var locationString = _locationStrings[i];
               // Debug.Log(locationString);
                _locations[i] = Conversions.StringToLatLon(locationString);
                var instance = Instantiate(_markerPrefab);

                var currentSafeManager =  instance.GetComponent<SafeManager>();
                //Debug.Log("CURRENTSAFEMANAGER: " + currentSafeManager);
                currentSafeManager.id = ids[i];
                //Debug.Log("CURRENTSAFEMANAGERID: " + currentSafeManager.id);
                currentSafeManager.hp = hps[i];
                currentSafeManager.level = levels[i];
                currentSafeManager.locationX = _locations[i].x; //locationString.Split(",")[0];
                currentSafeManager.locationY = _locations[i].y;
                currentSafeManager.status = statuses[i];
                currentSafeManager.isRobUnion = isRobunionList[i];
                if (isRobunionList[i] == 1)
                {
                    currentSafeManager.ActivateRobUnion();
                }
                instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i]);
                
                instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                _spawnedObjects.Add(instance);
            }
        }
        
        public void SpawnCubes()
        {
            _cubeLocations = new Vector2d[_locationStrings.Count];
            for (var i = 0; i < _locationStrings.Count; i++)
            {
                var location = _locationStrings[i];
                _cubeLocations[i] = Conversions.StringToLatLon(location);
                var pos = GameObject.CreatePrimitive(PrimitiveType.Cube);
                pos.GetComponent<Collider>().enabled = false;
                pos.GetComponent<MeshRenderer>().enabled = false;
                pos.transform.position = _map.GeoToWorldPosition(_cubeLocations[i], false);
                pos.transform.position = new Vector3(pos.transform.position.x, pos.transform.position.y + 2,
                    pos.transform.position.z);
                pos.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                cubes.Add(pos);
            }
        }

    }
}