using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class SafeDialogue : MonoBehaviour
{
    // Start is cal] led before the first frame update
    [SerializeField] private TextMeshProUGUI safeLevelText;
    [SerializeField] private TextMeshProUGUI locationText;
    [SerializeField] private GameObject heistPrepScreen;

    public void InitializeSafe(int level, double locationX, double locationY)
    {
        safeLevelText.text = "Level " + level + " Safe";
        locationText.text = "Coordinates: " + Math.Round(locationX, 4).ToString(CultureInfo.CurrentCulture).Replace(",", ".") + ", " + Math.Round(locationY, 4).ToString(CultureInfo.CurrentCulture).Replace(",", ".");
    }

    public void PressStartRobberyButton()
    {
        heistPrepScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    public void PressCancelButton()
    {
        gameObject.SetActive(false);
    }
}
