using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float Hmove = Input.GetAxis("Horizontal");
        float vMove = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(Hmove, 0, vMove);
        rb.AddForce(move);
        
    }
}
