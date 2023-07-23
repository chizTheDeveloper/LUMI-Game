using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShoot : MonoBehaviour
{
    public List<GameObject> bubbles;
    public GameObject playerFacing;
    public GameObject bub;
    public CharacterController playerPos;
    public Vector3 wayFacing;
    float timeStamp;
    public Player player;

    AudioSource shoot;
    public GameObject audioGO;

    private Animator animator;
    public GameObject manta;

    public GameObject enemy;

    public GameObject gameManager;

    void Start()
    {
        bubbles = new List<GameObject>();

        animator = manta.GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager");

        shoot = audioGO.GetComponent<AudioSource>();

    }

    void Update()
    {
        // If left mouse button is clicked
        if(!gameManager.GetComponent<PauseMenu>().activity){
        if((Input.GetMouseButtonDown(0) || Input.GetAxis("Shoot") != 0 ) && timeStamp <= Time.time && FindObjectOfType<DialogueManager>().dialogueText.enabled == false && gameObject.GetComponent<Movement>().gameBegins == true){
           Shoot();
           //if(player.SQ3 == true){
          // timeStamp = Time.time + 1.1f;
           //}else{
          //  timeStamp = Time.time + 1.5f;
          // }
        }
        }

        bubbles.RemoveAll(item => item == null);

        // Loop through bubbles
        //for(int i = 0; i < bubbles.Count; i++){
            // Keep sending the bubble in the direction that player was facing

    
           // bubbles[i].transform.Translate(wayFacing * Time.deltaTime * 80);
            

            // Get the distance between the player and the shot bubble
            //float distance = Vector3.Distance (bubbles[i].transform.position, playerPos.transform.position);


            //moved this to bubbleScript
            /*
            // If the bubble is over a certain distance away from the player, get rid of it
            if(player.SQ2 == true){
                if(distance > 100){
                    Destroy(bubbles[i]);

                }
            }else{
                if(distance > 80){
                Destroy(bubbles[i]);

            }
            }
            */

       // }

    
    }

     void Shoot(){
            animator.Play("attack",0,0);

            shoot.Play();

           // wayFacing = playerPos.transform.forward;

            bub = (GameObject)Instantiate(Resources.Load("BubbleFinal"));

            bub.GetComponent<bubbleScript>().player = player;
            bub.GetComponent<bubbleScript>().playerPos = playerPos;
        
            //playerFacing = GameObject.FindWithTag("Wayfacing");
            //wayFacing = playerFacing.transform.forward;
            
            // Change position of the bubble and its scale
            bub.transform.position = playerPos.transform.position;

            // Add the new instance of the bubble to the list to keep track of them
            bubbles.Add(bub);
            if(player.SQ3 == true){
                timeStamp = Time.time + 1.0f;
            }else{
                timeStamp = Time.time + 1.5f;
            }

            if(gameObject.GetComponent<Player>().currentXP == 5){
                Debug.Log("fullblast");
                bub.transform.localScale = new Vector3(5,5,5);
                //gameObject.GetComponent<Player>().currentXP = 0;
                //gameObject.GetComponent<Player>().xpBar.SetXP(0);
            }
            else{
                if(player.SQ1 == true){
                    bub.transform.localScale = new Vector3(3,3,3);
                }
                else{
                    bub.transform.localScale = new Vector3(2,2,2);
                }
            }
    }



}
