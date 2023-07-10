using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Utils;
using UnityEngine;

public class OtherPlayer : MonoBehaviour
{
    public int id;
    public bool role;
    public Vector2d location;
    public Quaternion rotation;
    private Canvas clickablePoliceman;

    public OtherPlayer(int id, bool role, Vector2d location, Quaternion rotation)
    {
        this.id = id;
        this.role = role;
        this.location = location;
        this.rotation = rotation;
    }

    private void Awake()
    {
        clickablePoliceman.worldCamera = Camera.main;
    }
} 
