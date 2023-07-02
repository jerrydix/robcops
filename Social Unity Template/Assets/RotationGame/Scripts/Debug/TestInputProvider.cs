using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInputProvider : MonoBehaviour
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float hp;
    [SerializeField]
    private float time;
    
    private RotateGameManager manager;

    private void Awake()
    {
        manager = GetComponent<RotateGameManager>();
    }

    void Start()
    {
        manager.Initialize(time, hp, damage);
    }
    
}
