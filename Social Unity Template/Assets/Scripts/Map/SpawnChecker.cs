using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;

public static class SpawnChecker
{

    public static bool CheckObjectFreePosition(GameObject obj, Vector3 position) //Assume position to be the center
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

    public static bool CheckIsFreePos(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.down, out hit, Mathf.Infinity))
        {
            GameObject other = hit.collider.gameObject;
            if (other.CompareTag("Building") || other.CompareTag("Safe"))
            {
                //Debug.Log("hit");
                return false;
            }
        }
        //Debug.Log("no hit");
        return true;
    }

    public static Vector3 ConvertPos(AbstractMap map, Vector2d cordinate)
    {
        return map.GeoToWorldPosition(cordinate, true);
    }


}

