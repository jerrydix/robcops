using System;
using System.Collections;

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
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		[Geocode]
		public string[] _locationStrings;
		Vector2d[] _locations;
		private Vector2d[] _cubeLocations;

		[SerializeField]
		float _spawnScale = 100f;

		private bool activateUpdate = false;
		

		
		public GameObject _markerPrefab;

		private SafeSpinScript SafeSpinScript;

		public List<GameObject> _spawnedObjects;
		private List<GameObject> cubes = new List<GameObject>();

		[SerializeField]
		private GameObject testo;
	

		void Start()
		{
			CheckCollision();
			StartCoroutine(waitTest());
		}

		private void FixedUpdate()
		{
			UpdateLocation();
		}

		private void Update()
		{
			if (activateUpdate)
			{
				safeLocation();
			}
		}

		private void safeLocation()
		{
			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
				
			}
		}

		private IEnumerator waitTest()
		{
			yield return new WaitForSeconds(5);
			_locations = new Vector2d[_locationStrings.Length];
			_spawnedObjects = new List<GameObject>();
			for (int i = 0; i < _locationStrings.Length; i++)
			{
				if (_locationStrings[i] == null)
				{
					continue;
				}
				var locationString = _locationStrings[i];
				_locations[i] = Conversions.StringToLatLon(locationString);
				var instance = Instantiate(_markerPrefab);
				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
				instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
				_spawnedObjects.Add(instance);

			}
			
        }



		private void UpdateLocation()
		{
			RaycastHit hit;
			int count = cubes.Count;
			for (int i = 0; i < count; i++)
			{
				var test = cubes[i];
				var location = _cubeLocations[i];
				test.transform.localPosition = _map.GeoToWorldPosition(location, true);
				test.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
				var temp = test.transform.position;
				temp.y += 5;
				if (Physics.Raycast(temp,Vector3.down,out hit,Mathf.Infinity))
				{
					if (hit.collider.gameObject.CompareTag("Building"))
					{
						Debug.Log(hit.collider.gameObject);
						_locationStrings[i] = null;
						Debug.Log("PLS");
					}
					
				}
			}
		}
		private void CheckCollision()
		{
			_cubeLocations = new Vector2d[_locationStrings.Length];
			for (int i = 0; i < _locationStrings.Length; i++)
			{
				
				var location = _locationStrings[i];
				_cubeLocations[i] = Conversions.StringToLatLon(location);
				var pos = GameObject.CreatePrimitive(PrimitiveType.Cube);
				pos.GetComponent<Collider>().enabled = false;
				pos.GetComponent<MeshRenderer>().enabled = false;
				pos.transform.position = _map.GeoToWorldPosition(_cubeLocations[i],true);
				pos.transform.position = new Vector3(pos.transform.position.x, pos.transform.position.y + 2,
					pos.transform.position.z);
				pos.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
				cubes.Add(pos);
				

			}
			
        }

		public bool CheckIsFreePos(Vector3 position)
		{
			RaycastHit hit;
			if(Physics.Raycast(position, Vector3.down, out hit, Mathf.Infinity))
			{
				if (hit.collider.gameObject.CompareTag("Building"))
				{
					Debug.Log("hit");
					return false;
				}
			}
			Debug.Log("no hit");
			return true;
		}

		public Vector3 ConvertPos(Vector2d cordinate)
		{
			return _map.GeoToWorldPosition(cordinate, true);
        }

		public void test()
		{
			Vector3 v = testo.gameObject.transform.position;
			v.y += 4;
			CheckIsFreePos(v);
		}
		
		
	}
	
}