using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP : MonoBehaviour
{

    public Player player;
    QuestTracker questTracker;
    public GameObject gameManager;
    AudioSource gemCollect;
    public GameObject gemSoundObj;

    void Start(){
        gemSoundObj = GameObject.FindWithTag("gemSound");
        gemCollect = gemSoundObj.GetComponent<AudioSource>();
    }

    void Update()
    {
         if(!gameManager.GetComponent<PauseMenu>().activity){
        Vector3 bobbing = new Vector3(transform.position.x, transform.position.y + ((float)Mathf.Sin(Time.time) * 0.08f), transform.position.z);
        transform.position = bobbing;

        transform.Rotate(0f, 1f, 0f, Space.Self);
         }
    }


    public void OnTriggerEnter(Collider other)
    {
       // PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if(other.CompareTag("Player")){

            if(player.currentXP < 5){
            gameObject.SetActive(false);
            player.GainXP(1);
            gemCollect.Play();

            questTracker = other.GetComponent<QuestTracker>();

            if(questTracker.xpFoundCount == 0){
                questTracker.progress.Add("foundXP");
                questTracker.interactingWith = "companion";
                questTracker.xpFoundCount++;
            }
            }
        }
           // playerInventory.XPCollected();
           

        
    }
}
