using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MovementController : MonoBehaviour
{

    float joystickX;
    float joystickY;

    public GameObject gameManager;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<Movement>().gameBegins == true && player.GetComponent<Movement>().freezeRotation == false){
            Move();
        }
    }

    public void Move()
    {

        // Getting mouse positions
        if(!gameManager.GetComponent<PauseMenu>().activity){


        joystickX += Input.GetAxis("Joystick X");
        joystickY += Input.GetAxis("Joystick Y");

         //map
         /*
         if(Input.GetKeyDown(interactKeyMAP)){
            if(mapOpen == false){
            mapBig.enabled = true;
            Debug.Log("OPEN MAP");
            mapOpen = true;
            }else{
            mapBig.enabled = false;
            mapOpen = false;
            }
            */
        }

        
        // Rotation using mouse position (to look around), x2 for faster rotation)
        transform.localRotation = Quaternion.Euler(-joystickY * 8f, joystickX * 8f, 0);

/*
        if(Input.GetKeyDown(KeyCode.Space)){
            boosting = true;
            StartCoroutine(BoostDuration(0.7f));
        }
        else{
            if(boosting == true){
                if(playerSpeed < 80){
                    // Quickly increase speed when boost has be used
                    playerSpeed = playerSpeed + 2;
                }
            }
            else{
                if(playerSpeed > 60){
                    // Slowly reduce speed when boost isn't in use
                    playerSpeed--;
                }
            }

            // Move player
            characterController.Move(playerSpeed * Time.deltaTime * move);

            if(move != Vector3.zero){
                animator.SetBool("isSwimming", true);
                animator2.SetBool("isSwimming", true);
            }else{
                animator.SetBool("isSwimming", false);
                animator2.SetBool("isSwimming", false);
            }
        }
        */


    }
}
