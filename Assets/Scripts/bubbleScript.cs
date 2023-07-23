using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubbleScript : MonoBehaviour
{

    public float TimeLeft;
    public float TimeLeft2;
    public Player player;
    public Vector3 wayFacing;
    public CharacterController playerPos;
    public Vector3 playerVect;
    Rigidbody rbBubble;

    // Start is called before the first frame update
    void Start()
    {
        wayFacing = playerPos.transform.forward;

        //playerVect = GameObject.FindWithTag("Wayfacing").transform.forward;

        //rbBubble = GetComponent<Rigidbody>();

        // Checking for special abilities
        if(player.SQ2 == true){
               TimeLeft = 1.8f;
        }else{
            TimeLeft = 1f;
        }

    }

    // we have to use FixedUpdate so lag in level 2 doesn't affect bubble speed
    void FixedUpdate()
    {
        TimeLeft -= Time.deltaTime;
        
    
        if ( TimeLeft < 0 )
        {
            Destroy(gameObject);
             if(player.GetComponent<Player>().currentXP == 5){
                //Debug.Log("fullblast");
                //bub.transform.localScale = new Vector3(5,5,5);
                player.GetComponent<Player>().currentXP = 0;
                player.GetComponent<Player>().xpBar.SetXP(0);
            }
        }

        Shoot();

    }

    void Shoot()
    {
        // Move the bubble in direction player was facing when it was shot
        transform.position += new Vector3(wayFacing.x, wayFacing.y - 0.05f, wayFacing.z) * 3f;
    }

    private void OnTriggerEnter(Collider collision)
     {


         if (collision.transform.tag == "enemy")
         {
            Destroy(gameObject);
            if(player.GetComponent<Player>().currentXP == 5){
                //Debug.Log("fullblast");
                //bub.transform.localScale = new Vector3(5,5,5);
                player.GetComponent<Player>().currentXP = 0;
                player.GetComponent<Player>().xpBar.SetXP(0);
            }
         }

         if (collision.transform.tag == "Kraken")
         {
            Destroy(gameObject);
            if(player.GetComponent<Player>().currentXP == 5){
                //Debug.Log("fullblast");
                //bub.transform.localScale = new Vector3(5,5,5);
                player.GetComponent<Player>().currentXP = 0;
                player.GetComponent<Player>().xpBar.SetXP(0);
            }
         }

         if (collision.transform.tag == "Weak")
         {
            Destroy(gameObject);
            if(player.GetComponent<Player>().currentXP == 5){
                //Debug.Log("fullblast");
                //bub.transform.localScale = new Vector3(5,5,5);
                player.GetComponent<Player>().currentXP = 0;
                player.GetComponent<Player>().xpBar.SetXP(0);
            }
         }

         if (collision.transform.tag == "Blob")
         {
            Destroy(gameObject);
            if(player.GetComponent<Player>().currentXP == 5){
                //Debug.Log("fullblast");
                //bub.transform.localScale = new Vector3(5,5,5);
                player.GetComponent<Player>().currentXP = 0;
                player.GetComponent<Player>().xpBar.SetXP(0);
            }
         }

         if (collision.transform.tag == "bossblob")
         {
            Destroy(gameObject);
            if(player.GetComponent<Player>().currentXP == 5){
                //Debug.Log("fullblast");
                //bub.transform.localScale = new Vector3(5,5,5);
                player.GetComponent<Player>().currentXP = 0;
                player.GetComponent<Player>().xpBar.SetXP(0);
            }
         }

    
     }
}
