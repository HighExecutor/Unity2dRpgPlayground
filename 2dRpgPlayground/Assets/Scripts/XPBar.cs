using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Text level;

    public void SetMaxExp(int maxExp)
    {
        slider.maxValue = maxExp;
        slider.value = maxExp;
    }

    public void SetExp(float exp)
    {
        slider.value = exp;
    }

    public void SetLevel(int newLevel)
    {
        level.text = "" + newLevel;
    }
}
