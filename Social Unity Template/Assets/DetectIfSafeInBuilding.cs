using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Examples;
using Mapbox.Unity.MeshGeneration.Data;
using Unity.VisualScripting;
using UnityEngine;

public class DetectIfSafeInBuilding : MonoBehaviour
{
   private SpawnOnMap _spawnOnMap;
   private List<GameObject> _yeetedSafes;

   private void Awake()
   {
      _yeetedSafes = new List<GameObject>();
      _spawnOnMap = GameObject.FindWithTag("Spawner").GetComponent<SpawnOnMap>();
   }

   void Start()
   {
      
   }

   


 


   /*
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("Building"))
      {
         //Debug.Log("Connection");
         //gameObject.Destroy();
      }
   }
   */

   private void OnTriggerExit(Collider other)
   {
      if (other.gameObject)
      {
         
      }
   }
}
