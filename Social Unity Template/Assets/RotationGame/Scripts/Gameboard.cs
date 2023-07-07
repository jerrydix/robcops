using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameboard : MonoBehaviour
{
    private GyrosInput gi;
    private Vector3 initObjRotationEuler;
    private Quaternion initObjRotation;
    private float accuAcc;
    private float maxAccuAcc = 10;


    private void Awake()
    {
        gi = GetComponent<GyrosInput>();
        initObjRotation = gameObject.transform.rotation;
        initObjRotationEuler = initObjRotation.eulerAngles;
    }

    private void Start()
    {
        accuAcc = 0;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        gameObject.transform.rotation = initObjRotation;
    }

    private void Update()
    {
        //UpdateByRate();
        UpdateByAccumulatedRotationRate();
    }

    private void FixedUpdate()
    {
        //UpdateByEuler();
        //UpdateByQuaternion();
    }

    //[Deprecated] Works, but very inaccurate
    private void UpdateByRate()
    {
        float rotRate = gi.GetRotationRate();
        gameObject.transform.Rotate(new Vector3(0, -rotRate * 2, 0));
    }

    //[Deprecated] Works good, as far as you are facing East or West
    private void UpdateByEuler()
    {
        var gyrosData = gi.GetRotation();
        Vector3 newRotation = new Vector3(gyrosData, initObjRotationEuler.y, initObjRotationEuler.z);
        gameObject.transform.rotation = Quaternion.Euler(newRotation);
    }

    //[Deprecated] Don't use
    private void UpdateByQuaternion()
    {
        gameObject.transform.rotation = gi.GetQuaternion();
    }

    //This is the way
    private void UpdateByAccumulatedRotationRate()
    {
        accuAcc -= gi.GetRotationRate() * Time.timeScale;
        if(accuAcc < -maxAccuAcc)
        {
            accuAcc = -maxAccuAcc;
        }
        if(accuAcc > maxAccuAcc)
        {
            accuAcc = maxAccuAcc;
        }

        gameObject.transform.Rotate(new Vector3(0, RotationFunctionRoot(accuAcc), 0));
    }

    private float RotationFunctionLinear(float input)
    {
        return input * Time.deltaTime * 15;
    }

    //Gain more control on smaller values - to easy imo
    private float RotationFunctionPolynom(float input)
    {
        if (input < 0)
        {
            input *= -1;
            return -Mathf.Pow(input, 1.5f) * Time.deltaTime * 5;
        }
        else
        {
            return Mathf.Pow(input, 1.5f) * Time.deltaTime * 5;
        }
    }

    //less control on smaller values
    private float RotationFunctionRoot(float input)
    {
        if(input < 0)
        {
            input *= -1;
            return -Mathf.Pow(input, 0.6f) * Time.deltaTime * 40;
        }
        else
        {
            return Mathf.Pow(input, 0.6f) * Time.deltaTime * 40;
        }
    }
}
