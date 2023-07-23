using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBlobs : MonoBehaviour
{

    public Transform player;
    public GameObject playerObj;
    Vector3 dir;
    Vector3 movement;
    public CharacterController Char;

    public GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        Char = gameObject.GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        dir = player.position - transform.position;
        movement = dir.normalized * 1.5f;

        if(movement.magnitude > dir.magnitude) movement = dir;

        if(!FindObjectOfType<PauseMenu>().activity && FindObjectOfType<DialogueManager>().dialogueText.enabled == false){
        Char.Move(movement);
        }
        //transform.LookAt(player);
        //transform.position = Vector3.MoveTowards(transform.position, player.position, 1.5f);
    }


     void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (collision.gameObject.tag == "Player"){
            playerObj.GetComponent<Player>().currentHealth -= 1;
            playerObj.GetComponent<Player>().healthBar.SetHealth(playerObj.GetComponent<Player>().currentHealth);
            Destroy(gameObject);
        }
    }
    
}
