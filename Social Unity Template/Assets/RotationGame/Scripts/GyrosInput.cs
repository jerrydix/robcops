using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyrosInput : MonoBehaviour
{
    //rotation we want is z

    private Gyroscope gyro;
    private float initDeviceRotation_z;

    private void Awake()
    {
        gyro = Input.gyro;
        gyro.enabled = true;
        initDeviceRotation_z = gyro.attitude.eulerAngles.z;
    }


    private float ConvertAngle(float rawAngle)
    {
        float angle = (rawAngle + 90) % 360;
        angle = Mathf.Abs(angle - initDeviceRotation_z);
        
        return angle;
    }
    
    public float GetRotation()
    {
        return ConvertAngle(gyro.attitude.eulerAngles.z);
    }

    public float GetRotationRate()
    {
        return gyro.rotationRateUnbiased.z;
    }

    public Quaternion GetQuaternion()
    {
        return gyro.attitude;
    }

    
}
