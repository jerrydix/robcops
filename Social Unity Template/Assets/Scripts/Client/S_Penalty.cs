using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Penalty : MonoBehaviour
{
    public SafeManager safeManager;

    public void yes()
    {
        safeManager.arrest(1);
    }
    
    public void no()
    {
        safeManager.arrest(0);
    }
}
