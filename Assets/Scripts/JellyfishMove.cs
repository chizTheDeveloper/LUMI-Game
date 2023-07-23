using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishMove : MonoBehaviour
{

    public GameObject player;
    public GameObject thisJelly;

    float distanceBetween;
    float startY;

    Vector3 jellyPos;
    Vector3 velocity = Vector3.one;

    public Rigidbody rb;
    QuestTracker questTracker;
    Movement movement;

    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
        rb = GetComponent<Rigidbody>();
        questTracker = player.GetComponent<QuestTracker>();

        movement = player.GetComponent<Movement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Get position of jellyfish and get the distance between it and the player
        jellyPos = transform.position;
        distanceBetween = Vector3.Distance(transform.position, player.transform.position);

        // If player close enough to jellyfish, catch it and destroy gameobject
        if(distanceBetween <= 15){
            Destroy(thisJelly);
            
            // Keeping track of how many jellyfish have been caught
            questTracker.jellyCaught = questTracker.jellyCaught + 1;
            movement.newJelly = true;
        }
        // If the player is close to jellyfish but not close enough to catch it, make the jellyfish run away
        else if(distanceBetween < 70 && distanceBetween > 10){
            Vector3 moveTo = transform.position - player.transform.position;
            rb.AddRelativeForce(player.transform.forward * 30);
        }
        // If the jellyfish is far from the player, stop the jellyfish from moving
        else{
            rb.Sleep();
        }
    }
}
