using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExtraLife : MonoBehaviour
{

    QuestTracker questTracker;
    QuestTracker2 questTracker2;
    QuestTracker3 questTracker3;

    public bool heartCollected;

    public GameObject heart;

    public Player player;

    void Start()
    {
        heartCollected = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

            if(player.numOfHearts == 5){
            player.custumText5.enabled = false;
            player.custumText6.enabled = true;
            Destroy(gameObject);
            
            }

            if(player.numOfHearts == 4){
            player.custumText4.enabled = false;
            player.custumText5.enabled = true;
            Destroy(gameObject);
            
            }


             if(player.numOfHearts == 3){
            player.custumText3.enabled = false;
            player.custumText4.enabled = true;
            Destroy(gameObject);
            }

            if(player.numOfHearts == 2){
            player.custumText2.enabled = false;
            player.custumText3.enabled = true;
            Destroy(gameObject);
            }


             if(player.numOfHearts == 1){
            player.custumText.enabled = false;
            player.custumText2.enabled = true;
            Destroy(gameObject);

            }

            if(player.numOfHearts == 0){
                player.custumText.enabled = true;
                
                Destroy(gameObject);
            }

            questTracker = other.GetComponent<QuestTracker>();
            questTracker2 = other.GetComponent<QuestTracker2>();
            questTracker3 = other.GetComponent<QuestTracker3>();

            if(questTracker != null){
                questTracker.progress.Add("foundHeart");
                questTracker.interactingWith = "companion";
            }
            else if(questTracker2 != null){
                questTracker2.progress.Add("foundHeart");
                questTracker2.interactingWith = "companion";
            }
            else if(questTracker3 != null){
                questTracker3.progress.Add("foundHeart");
                questTracker3.interactingWith = "companion";
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player")){
          //  custumImage.enabled = false;
        }
    }

    void Update(){
        Vector3 bobbing = new Vector3(transform.position.x, transform.position.y + ((float)Mathf.Sin(Time.time) * 0.02f), transform.position.z);
        transform.position = bobbing;

        transform.Rotate(0f, 1f, 0f, Space.Self);
    }
}
