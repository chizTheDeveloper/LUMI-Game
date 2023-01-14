using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
      public Slider slider2;

    public void SetMaxHealth(int healthEnemy)
    {
        slider2.maxValue = healthEnemy;
        slider2.value = healthEnemy;
    }
    
    public void SetHealth(int healthEnemy)
    {
        slider2.value = healthEnemy;
    }
}
