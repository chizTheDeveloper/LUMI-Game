using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    //variables
    public Transform player;
    public float mouseSensitivity = 2f;
    float cameraVerticalRotation = 0f;


    bool lockedCursor = true;

    // Start is called before the first frame update
    void Start()
    {
        
        //lock and hide the cursor
        Cursor.visible = false;
      //  Cursor.lockstate =
    }

    // Update is called once per frame
    void Update()
    {
        //collect Input
        float inputX = Input.GetAxis("Mouse X")*mouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y")*mouseSensitivity;

        //Rotate the camera around its local x axis
        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        //Rotate the player object and the camera around its Y axis
        player.Rotate(Vector3.up * inputX);
    }
}

