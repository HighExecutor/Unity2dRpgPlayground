using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldBar : MonoBehaviour
{
    public Text goldAmount;

    public void SetGold(float gold)
    {
        goldAmount.text = "" + gold;
    }
}
