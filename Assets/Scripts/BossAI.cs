using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class BossAI : MonoBehaviour
{

    public GameObject kraken;
    public Player player;
    public int damage;
    Movement movement;
    BoxCollider2D headCol;
    public GameObject weakSpot;
    public QuestTrackerBoss questTrackerBoss;

    private Animator animator;
    bool hasDied;

    // Start is called before the first frame update
    void Start()
    {
        movement = player.GetComponent<Movement>();
        animator = kraken.GetComponent<Animator>();
        questTrackerBoss = player.GetComponent<QuestTrackerBoss>();
        hasDied = false;
    }

    // Update is called once per frame
    void Update()
    {
        movement.gameBegins = true;
        if(player.currentXP == 5){
            damage = 8;
         }else if(player.SQ1 == false){
            damage = 1;
         }else if(player.SQ1 == true){
            damage = 2;
         }


         if(gameObject.GetComponent<BoosHealth>().currentHealth <= 0){
            Debug.Log("dead");
            questTrackerBoss.progress.Add("defeat");
            questTrackerBoss.interactingWith = "kraken";
            hasDied = true;
            //SceneManager.LoadScene("CREDITS");
        }

        if(questTrackerBoss.progress.LastOrDefault() == "dead"){
            //Destroy(gameObject);

            // Add code to destory the kraken gameobject pieces
        }

        if(FindObjectOfType<DialogueManager>().dialogueText.enabled == true){
            animator.speed = 1;
        }else if(FindObjectOfType<DialogueManager>().dialogueText.enabled == false){
            animator.speed = 1;
        }
    }

     private void OnTriggerEnter(Collider collision)
     {
         if (collision.transform.tag == "bubble")
         {
             // do damage here, for example:
             //Debug.Log(collision.transform.name);
             gameObject.GetComponent<BoosHealth>().TakeDamage(damage);
         }
     }
}
