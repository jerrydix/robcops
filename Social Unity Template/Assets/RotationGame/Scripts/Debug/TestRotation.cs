using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotation : MonoBehaviour
{
    private void FixedUpdate()
    {
        gameObject.transform.Rotate(new Vector3(0, 10, 0) * Time.fixedDeltaTime);
    }
}
