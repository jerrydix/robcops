using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnChecker : MonoBehaviour
{
    [SerializeField]
    AbstractMap _map;

    [SerializeField]
    private GameObject testo;

    private int two = 2;


    private void Update()
    {
       
        //test();
    }

    public bool CheckObjectFreePosition(GameObject obj, Vector3 position) //Assume position to be the center
    {
        Vector3[] points = new Vector3[4];
        Vector3 scale = obj.transform.localScale;
        points[0] = new Vector3(position.x - (scale.x / 2), 10, position.z - (scale.z / 2));
        points[1] = new Vector3(position.x + (scale.x / 2), 10, position.z - (scale.z / 2));
        points[2] = new Vector3(position.x - (scale.x / 2), 10, position.z + (scale.z / 2));
        points[3] = new Vector3(position.x + (scale.x / 2), 10, position.z + (scale.z / 2));
        //Debug.Log(points[0]);
        for(int i = 0; i < 4; i++)
        {
            if (!CheckIsFreePos(points[i]))
            {
                return false;
            }
        }
        return true;
    }

    public bool CheckIsFreePos(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.down, out hit, Mathf.Infinity))
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
        CheckObjectFreePosition(testo, testo.transform.position);
        //CheckIsFreePos(testo.transform.position);
    }


}

