using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInputProvider : MonoBehaviour
{

    private RotateGameManager manager;

    private void Awake()
    {
        manager = GetComponent<RotateGameManager>();
    }

    void Start()
    {
        manager.Initialize();
    }
    
}
