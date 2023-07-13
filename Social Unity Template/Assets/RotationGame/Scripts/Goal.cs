using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    RotateGameManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            manager.rotationGameUIManager.DamageSafe();
            if (!manager.rotationGameUIManager.gameComplete)
            {
                manager.scene.SetActive(false);
                manager.PickRandomScene();
                manager.ResetBall();
            }
            
        }
    }
}
