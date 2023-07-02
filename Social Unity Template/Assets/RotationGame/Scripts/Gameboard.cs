using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameboard : MonoBehaviour
{
    private GyrosInput gi;
    private Vector3 initObjRotation;

    private void Awake()
    {
        gi = GetComponent<GyrosInput>();
        initObjRotation = gameObject.transform.rotation.eulerAngles;
    }

    private void FixedUpdate()
    {
        var gyrosData = gi.GetRotation();
        Vector3 newRotation = new Vector3(gyrosData, initObjRotation.y, initObjRotation.z);
        gameObject.transform.rotation = Quaternion.Euler(newRotation);
        //Euler Properties lead to weird consequences: Need a way to fix
    }
}
