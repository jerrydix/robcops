using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class DebugOrientation : MonoBehaviour
{
    [SerializeField]
    private Text debug;


    private void FixedUpdate()
    {
        Vector3 current = gameObject.transform.rotation.eulerAngles;
        debug.text = Input.gyro.attitude.eulerAngles.ToString() + "\n" + Input.gyro.rotationRateUnbiased + "\n" + current.ToString()
                    + "\n" + Input.gyro.attitude.ToString();
    }

}
