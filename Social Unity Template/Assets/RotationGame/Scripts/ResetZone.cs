using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetZone : MonoBehaviour
{
    [SerializeField] private RotateGameManager manager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball")){
            manager.ResetBall();
        }
    }
}
