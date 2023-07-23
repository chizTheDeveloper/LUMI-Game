using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosHealth : MonoBehaviour
{
   public EnemyHealthBar healthBar;
    public int maxHealth;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        //healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    { 
        if(Input.GetKeyDown("e")){
            currentHealth = 0;
        }
        
    }

    public void TakeDamage(int damageAmount)
         {
             currentHealth -= damageAmount;
             //healthBar.SetHealth(currentHealth);
             //Debug.Log(damageAmount);
             // other stuff you want to happen when enemy takes damage
         }

}
