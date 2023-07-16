using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_MoneyRob_Check : MonoBehaviour
{
    public S_RobUnionController controller;
    public TextMeshProUGUI moneyText;

    // Update is called once per frame
    void Update()
    {
        int money = controller.machinesCount * 100;
        moneyText.text = money + "K/Hour";
    }
}
