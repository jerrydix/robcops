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
    private UIManager _uiManager;
    private GameObject player;

    public OtherPlayer(int id, bool role, Vector2d location, Quaternion rotation)
    {
        this.id = id;
        this.role = role;
        this.location = location;
        this.rotation = rotation;
    }

    private void Awake()
    {
        if (role)
        {
            clickablePoliceman = GetComponent<Canvas>();
            clickablePoliceman.worldCamera = Camera.main;
        }
    }

    private void Start()
    {
        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        player = GameObject.FindWithTag("Player");
    }

    public void PressCopButton()
    {
        if (!GameManager.Instance.role && getDistanceToOtherPlayer() <= 20f)
        {
            Debug.Log("Cop pressed");
            _uiManager.ActivateCorruptionDialogue(id);
        }
    }

    public float getDistanceToOtherPlayer()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        return dist;
    }
} 
