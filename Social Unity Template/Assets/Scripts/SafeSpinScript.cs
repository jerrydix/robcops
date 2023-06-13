using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SafeSpinScript : MonoBehaviour
{
    //[SerializeField] private float amplitude = 1.0f;

    [SerializeField] private float rotationSpeed = 50f;

    //[SerializeField] private float frequency = 0.2f;
    private Safe _safe;
    public List<GameObject> blackList;

    // Update is called once per frame

    private void Start()
    {
        blackList = new List<GameObject>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
    }

   
}
