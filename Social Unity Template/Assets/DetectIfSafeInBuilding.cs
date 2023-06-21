using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.MeshGeneration.Data;
using Unity.VisualScripting;
using UnityEngine;

public class DetectIfSafeInBuilding : MonoBehaviour
{
   

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("Building"))
      {
         Debug.Log("Connection");
         gameObject.Destroy();
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.gameObject)
      {
         
      }
   }
}
