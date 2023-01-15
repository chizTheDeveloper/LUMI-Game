using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

// Works with cube
public class Movement : MonoBehaviour
{
    public CharacterController characterController;
    public float playerSpeed = 5000;
    float mouseX;
    float mouseY;

    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        //move
        Move();
    }

    public void Move()
    {
        // For WASD movement
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Getting mouse positions
        mouseX += Input.GetAxis("Mouse X");
        mouseY += Input.GetAxis("Mouse Y");

        // Vector 3 to move player
        Vector3 move = transform.forward * moveY + transform.right * moveX;
        
        // Rotation using mouse position (to look around), x2 for faster rotation)
        transform.localRotation = Quaternion.Euler(-mouseY * 0.6f, mouseX * 0.6f, 0);

        // Move the player based on controls 
        characterController.Move(playerSpeed * Time.deltaTime * move);
    }
}
