using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShoot : MonoBehaviour
{
    public List<GameObject> bubbles;
    public GameObject bub;
    public CharacterController playerPos;
    public Vector3 wayFacing;

    void Start()
    {
        bubbles = new List<GameObject>();
    }

    void Update()
    {
        // If left mouse button is clicked
        if(Input.GetMouseButtonDown(0)){
            wayFacing = playerPos.transform.forward;

            // Create instance of bubble
            bub = (GameObject)Instantiate(Resources.Load("bubbleShoot2"));

            // Change position of the bubble and its scale
            bub.transform.position = playerPos.transform.position;
            bub.transform.localScale = new Vector3(45, 45, 45);

            // Add the new instance of the bubble to the list to keep track of them
            bubbles.Add(bub);
        }

        bubbles.RemoveAll(item => item == null);

        // Loop through bubbles
        for(int i = 0; i < bubbles.Count; i++){
            // Keep sending the bubble in the direction that player was facing
            bubbles[i].transform.Translate(wayFacing * Time.deltaTime * 25);

            // Get the distance between the player and the shot bubble
            float distance = Vector3.Distance (bubbles[i].transform.position, playerPos.transform.position);

            // If the bubble is over a certain distance away from the player, get rid of it
            if(distance > 25){
                Destroy(bubbles[i]);
            }
        }
    }
}
