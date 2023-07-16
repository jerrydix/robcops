using Mapbox.Map;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;

public static class SpawnChecker
{

    public static bool CheckObjectFreePosition(GameObject obj, Vector3 position) //Assume position to be the center
    {
        Vector3[] points = new Vector3[4];
        Vector3 dimension = obj.GetComponent<Collider>().bounds.extents;
        points[0] = new Vector3(position.x + dimension.x, position.y + dimension.y, position.z + dimension.z);
        points[1] = new Vector3(position.x + dimension.x, position.y + dimension.y, position.z - dimension.z);
        points[2] = new Vector3(position.x - dimension.x, position.y + dimension.y, position.z + dimension.z);
        points[3] = new Vector3(position.x - dimension.x, position.y + dimension.y, position.z - dimension.z);
        //Debug.Log(points[0]);
        for (int i = 0; i < points.Length; i++)
        {
            if (!CheckIsFreePos(points[i]))
            {
                return false;
            }
        }
        return true;
    }

    public static bool CheckObjectFreePosition(GameObject obj, AbstractMap map, string locationX, string locationY) //Assume position to be the center
    {
        Vector3 pos = ConvertPos(map, locationX, locationY);
        return CheckObjectFreePosition(obj, pos);
    }

    public static bool CheckIsFreePos(Vector3 position)
    {
        position += new Vector3(0, 10, 0);
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

    public static bool CheckIsFreePos(AbstractMap map, string locationX, string locationY)
    {
        Vector3 pos = ConvertPos(map, locationX, locationY);
        return CheckIsFreePos(pos);
    }

    public static Vector3 ConvertPos(AbstractMap map, Vector2d cordinate)
    {
        return map.GeoToWorldPosition(cordinate, true);
    }

    public static Vector3 ConvertPos(AbstractMap map, string locationX, string locationY)
    {
        var locString = locationX + "," + locationY;
        var loc2d = Conversions.StringToLatLon(locString);
        return ConvertPos(map, loc2d);
    }


}

