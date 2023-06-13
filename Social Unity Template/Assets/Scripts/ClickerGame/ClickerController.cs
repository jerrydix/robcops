using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class ClickerController : MonoBehaviour
{
    private Safe clickerSafe;
    
    [SerializeField] private GameObject safe;
    [SerializeField] private GameObject hpBar;


    private int safeHp1 = 30;
    private int safeHp2 = 90;
    private int safeHp3 = 300;
    private int safeHp4 = 1000;
    public int safeHp;
    private int UnchangedSaveHp;
    

    // Start is called before the first frame update
    void Start()
    {
        clickerSafe = new Lv1Safe(); //Todo: Get Type of Safe based on location
        print(clickerSafe.GetType().ToString());
        switch (clickerSafe.GetType().ToString())
        
        {
            case "Lv1SaFe":
                safeHp = safeHp1;
                break;
            case "Lv2Safe":
                safeHp = safeHp2;
                break;
            case "Lv3Safe":
                safeHp = safeHp3;
                break;
            case "Lv4Safe":
                safeHp = safeHp4;
                break;
        }

        UnchangedSaveHp = safeHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (safeHp == 0)
        {
            return;
        }
        if (Input.touchCount != 0)
        {
            if (IsTouchOnTarget())
            {
                safeHp -= 1;
            }
        }

        hpBar.transform.localScale = new Vector3(1,(safeHp / UnchangedSaveHp) * hpBar.transform.localScale.y,1);
    }

    private bool IsTouchOnTarget()
    {
        // Create a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit;

        // Check if the ray intersects with the target GameObject
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == safe)
            {
                return true;
            }
        }

        return false;
    }
}