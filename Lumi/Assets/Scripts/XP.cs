using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP : MonoBehaviour
{

    public Player player;


    public void OnTriggerEnter(Collider other)
    {
       // PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

      
           // playerInventory.XPCollected();
            gameObject.SetActive(false);
            player.GainXP(1);

        
    }
}
