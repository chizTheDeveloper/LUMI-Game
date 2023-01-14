using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public int maxHealth = 20;
    public int currentHealth;

    public int maxXP = 5;
    public int currentXP;

    public XPBar xpBar;

    public HealthBar healthBar;

    public float attackSpeed = 1f;
    private float attackCooldown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        currentXP = 0;
        xpBar.SetMaxXP(maxXP);
        xpBar.SetXP(currentXP);
    }

    // Update is called once per frame
    void Update()
    {
        attackCooldown -= Time.deltaTime;
    
    }

    public void TakeDamage(int damage)
    {
        if (attackCooldown <= 0f)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            attackCooldown = 1f/attackSpeed;
        }
        
    }

    public void GainXP(int gain)
    {
        currentXP += gain;
        xpBar.SetXP(currentXP);
    }
}
