using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWallpaper : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
