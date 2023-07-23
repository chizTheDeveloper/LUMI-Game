using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    
    public Slider slider;

    public void SetMaxXP(int XP)
    {
        slider.maxValue = XP;
        slider.value = XP;
    }

    public void SetXP(int XP)
    {
        slider.value = XP;
    }


}
