using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lv1Safe : Safe
{
      public Lv1Safe() : base()
      {
        level = 1;
        moneyPerDay = 20000.0f;
        minTimeRequired = 86400.0f; //1 day in s
      }
}
