using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DetectSafes : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject safe;


    // Update is called once per frame
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(this);
            gameObject.SetActive(this);
            Debug.Log("Spawned");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            Destroy(this);
            Debug.Log("Not Spawned");
        }
    }

    private Vector3 calculateDistance()
    {
        return new Vector3(safe.transform.position.x - transform.position.x,
            safe.transform.position.y - transform.position.y,safe.transform.position.z - transform.position.z);
    }
}
