using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_MachinesOpen : MonoBehaviour
{
    private S_RobUnionController _robUnionController;
    void Start()
    {
        _robUnionController = GameObject.FindWithTag("Rob_C").GetComponent<S_RobUnionController>();
        Debug.Log(_robUnionController);
    }

    public void open()
    {
        Debug.Log(_robUnionController);

        _robUnionController.openMachines();
    }
}
