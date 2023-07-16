using System.Collections;
using System.Collections.Generic;
using Mapbox.Utils;
using UnityEngine;

public class C_OtherPlayerInfo
{
    public int id;
    public bool role;
    public Vector2d location;
    public Quaternion rotation;

    public C_OtherPlayerInfo(int id, bool role, Vector2d location, Quaternion rotation)
    {
        this.id = id;
        this.role = role;
        this.location = location;
        this.rotation = rotation;
    }
}
