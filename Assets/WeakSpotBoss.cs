using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakSpotBoss : MonoBehaviour
{
    public Player player;
    public int damage;
    public GameObject boss;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.currentXP == 5){
            damage = 8;
         }else if(player.SQ1 == false){
            damage = 3;
         }else if(player.SQ1 == true){
            damage = 5;
         }
         else{
            damage = 3;
         }
    }

    private void OnCollisionEnter(Collision collision){
         if(collision.transform.tag == "bubble"){
             Debug.Log("Hit weak spot");
             boss.GetComponent<BoosHealth>().TakeDamage(damage);
         }
     }
}
