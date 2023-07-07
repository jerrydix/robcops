using System;
using System.Collections;
using Mapbox.Unity.Location;

namespace Mapbox.Examples
{
    using UnityEngine;
    using Mapbox.Utils;
    using Mapbox.Unity.Map;
    using Mapbox.Unity.MeshGeneration.Factories;
    using Mapbox.Unity.Utilities;
    using System.Collections.Generic;

    public class SpawnOnMap : MonoBehaviour
    {
        [SerializeField] AbstractMap _map;

        [SerializeField] [Geocode] public List<string> _locationStrings;
        Vector2d[] _locations;
        private Vector2d[] _cubeLocations;

        [SerializeField] float _spawnScale = 100f;

        private static bool activateUpdate = false;


        public GameObject _markerPrefab;

        private SafeSpinScript SafeSpinScript;

        public List<GameObject> _spawnedObjects;
        private List<GameObject> cubes = new List<GameObject>();
        [SerializeField] private GameObject testo;
        private LocationArrayEditorLocationProvider _locationArrayEditorLocationProvider;
        private ImmediatePositionWithLocationProvider _immediatePositionWithLocationProvider;

        private GameManager gm;
        public List<int> ids = new List<int>();
        public List<int> levels = new List<int>();
        public List<int> hps = new List<int>();

        void Start()
        {
            CreateSafeIfNoneAreInRange();
            gm = GameObject.FindWithTag("GM").GetComponent<GameManager>();
            _locationArrayEditorLocationProvider = GameObject.FindWithTag("EditorOnly")
                .GetComponent<LocationArrayEditorLocationProvider>();
            _immediatePositionWithLocationProvider = GameObject.FindWithTag("Player")
                .GetComponent<ImmediatePositionWithLocationProvider>();
        }

        public void waitForCubeLocationThenSpawnSafe()
        {
            StartCoroutine(WaitForCubeLocationThenSpawnSafe());
        }

        private void FixedUpdate()
        {
            StartCoroutine(UpdateCubeLocation());
        }

        private void Update()
        {
            UpdateSafeLocation();
        }

        private void UpdateSafeLocation()
        {
            int count = _spawnedObjects.Count;
            for (int i = 0; i < count; i++)
            {
                if (_spawnedObjects[i] == null)
                {
                    continue;
                }

                var spawnedObject = _spawnedObjects[i];
                var location = _locations[i];
                spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
                spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                //Debug.Log("SpawnedSafe");
            }
        }

        public IEnumerator WaitForCubeLocationThenSpawnSafe()
        {
            yield return new WaitForSeconds(1);
            _locations = new Vector2d[_locationStrings.Count];
            _spawnedObjects = new List<GameObject>();
            for (int i = 0; i < _locationStrings.Count; i++)
            {
                var locationString = _locationStrings[i];
                Debug.Log(locationString);
                _locations[i] = Conversions.StringToLatLon(locationString);
                var instance = Instantiate(_markerPrefab);

                var currentSafeManager = _markerPrefab.GetComponent<SafeManager>();
                currentSafeManager.id = ids[i];
                currentSafeManager.hp = hps[i];
                currentSafeManager.level = levels[i];
                currentSafeManager.locationX = _locations[i].x; //locationString.Split(",")[0];
                currentSafeManager.locationY = _locations[i].y;

                instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);


                instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                _spawnedObjects.Add(instance);
            }
        }

        private IEnumerator UpdateCubeLocation()
        {
            yield return new WaitForSeconds(1);
            int count = cubes.Count;
            for (int i = 0; i < count; i++)
            {
                var test = cubes[i];
                var location = _cubeLocations[i];
                test.transform.position = new Vector3(_map.GeoToWorldPosition(location, true).x, 3,
                    _map.GeoToWorldPosition(location, true).z);
                test.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                if (!CheckIsFreePos(test.transform.position) |
                    !CalculateDistanceInEditor(location))
                {
                    _locationStrings[i] = null;
                    _spawnedObjects[i].Destroy();
                    _spawnedObjects[i] = null;
                }
            }

            activateUpdate = true;
        }

        public void SpawnCubes()
        {
            
            _cubeLocations = new Vector2d[_locationStrings.Count];
            for (int i = 0; i < _locationStrings.Count; i++)
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
        

        private bool CalculateDistanceToNorm(Vector2d location)
        {
            for (int i = 0; i < _locationStrings.Count; i++)
            {
                if (_locationStrings[i] == null)
                {
                    continue;
                }
                //Get Locations of Safe and Player

                //var currentString = _locationStrings[i];
                var instance = location;
                var x = Conversions.StringToLatLon(_immediatePositionWithLocationProvider.LocationProvider
                    .CurrentLocation.LatitudeLongitude.ToString());
                double playerLocation = x.x;
                double playerLocationy = x.y;

                //Calculate the Distance

                var deltaLat = (instance.x - playerLocation) * Mathd.PI / 180;
                var deltaLon = (instance.y - playerLocationy) * Mathd.PI / 180;

                var calc = (Mathd.Pow(Mathd.Sin(deltaLat / 2), 2) + Mathd.Cos(playerLocation * Mathd.PI / 180)
                    * Mathd.Cos(instance.x * Mathd.PI / 180) * Mathd.Pow(Mathd.Sin(deltaLon / 2), 2));
                var temp = 2 * Mathd.Atan2(Mathd.Sqrt(calc), Mathd.Sqrt(1 - calc));
                var result = 6371 * temp;
                result *= 1000;
                var finalResult = Mathd.Abs(result);

                //Filter Safes that are more than 1km away

                if (finalResult <= 350)
                {
                    return true;
                }
            }

            return false;
        }
        


        private bool CalculateDistanceInEditor(Vector2d location)
        {
            for (int i = 0; i < _locationStrings.Count; i++)
            {
                if (_locationStrings[i] == null)
                {
                    continue;
                }
                //Get Locations of Safe and Player

                //var currentString = _locationStrings[i];
                var instance = location;
                var x = Conversions.StringToLatLon(_locationArrayEditorLocationProvider._latitudeLongitude[0]);
                double playerLocation = x.x;
                double playerLocationy = x.y;

                //Calculate the Distance

                var deltaLat = (instance.x - playerLocation) * Mathd.PI / 180;
                var deltaLon = (instance.y - playerLocationy) * Mathd.PI / 180;

                var calc = (Mathd.Pow(Mathd.Sin(deltaLat / 2), 2) + Mathd.Cos(playerLocation * Mathd.PI / 180)
                    * Mathd.Cos(instance.x * Mathd.PI / 180) * Mathd.Pow(Mathd.Sin(deltaLon / 2), 2));
                var temp = 2 * Mathd.Atan2(Mathd.Sqrt(calc), Mathd.Sqrt(1 - calc));
                var result = 6371 * temp;
                result *= 1000;
                var finalResult = Mathd.Abs(result);

                //Filter Safes that are more than 1km away

                if (finalResult <= 350)
                {
                    return true;
                }
            }

            return false;
        }

        private void CreateSafeIfNoneAreInRange()
        {
            int result = 0;
            for (int i = 0; i < _cubeLocations.Length; i++)
            {
                if (CalculateDistanceInEditor(_cubeLocations[i]))
                {
                    result++;
                }
            }

            if (result != 0)
            {
                return;
            }

            var position = _immediatePositionWithLocationProvider.LocationProvider.CurrentLocation.LatitudeLongitude;
            global::Unity.Mathematics.Random random = new global::Unity.Mathematics.Random();
            int num = random.NextInt(0, 6);

            double deviations = random.NextDouble(0.1, 0.6);
            double deviations1 = random.NextDouble(0.1, 0.6);

            for (int i = 0; i < num; i++)
            {
                position = new Vector2d(position.x + deviations, position.y + deviations1);
                var LonLat = position.x + "," + position.y;
                _locationStrings.Add(LonLat);
            }
        }

        public bool CheckIsFreePos(Vector3 position)
        {
            RaycastHit hit;
            if (Physics.Raycast(position, Vector3.down, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.CompareTag("Building"))
                {
                    //Debug.Log("hit");
                    return false;
                }
            }

            //Debug.Log("no hit");
            return true;
        }

        public void test()
        {
            Vector3 v = testo.gameObject.transform.position;
            v.y += 4;
            CheckIsFreePos(v);
        }
    }
}