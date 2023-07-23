using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 velocity = Vector3.one;
    public Vector3 offset;
    public Vector3 defaultOffset;
    Vector3 origOffset;
    
    public Transform cameraT;
    public Transform target;

    Vector3 moveTo;
    Vector3 currentPos;
    float movementNumber;
    public bool changeLookAt;
    public bool pushCam;

    AudioSource battleMusic;
    public GameObject battleMusObj;

    void Start()
    {
        // Offset for camera position
        offset = cameraT.position - target.position;
        defaultOffset = offset;
        origOffset = defaultOffset;
        movementNumber = 0.3f;
        changeLookAt = false;
        pushCam = true;

        battleMusic = battleMusObj.GetComponent<AudioSource>();
    }

    void Awake(){
        // Get the camera transform
        cameraT = transform;
    }
    // LateUpdate maybe
    void Update()
    {
       Follow();

    }

    public void Follow(){
        // Position to move the camera
        moveTo = target.position + (target.rotation * offset);
        
        // If A or D are being pressed (moving side to side), reduce value used for smoothing the camera movement to prevent it from "dragging" to the side of player
        if((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetAxisRaw("HorizontalJoystick") != 0) && FindObjectOfType<DialogueManager>().dialogueText.enabled == false){
            if(movementNumber >= 0.08f){
               movementNumber = movementNumber - 0.04f;
            }

            // Move camera further away from player when moving side to side
            defaultOffset = Vector3.Lerp(offset, defaultOffset.normalized * 50, 1.0f - Mathf.Pow(0.5f, Time.deltaTime * 70f));
        }
        else if(Input.GetKey(KeyCode.S) && FindObjectOfType<DialogueManager>().dialogueText.enabled == false){
            defaultOffset = Vector3.Lerp(offset, defaultOffset.normalized * 55, 1.0f - Mathf.Pow(0.5f, Time.deltaTime * 100f));
        }
        // If moving forward or backward, increase the value used for smoothing
        else{
            if(movementNumber < 0.23f){
                movementNumber = movementNumber + 0.03f;
            }

            // Bring camera close to player again when they're not moving side to side
            offset = Vector3.Lerp(offset, defaultOffset, 1.0f - Mathf.Pow(0.5f, Time.deltaTime * 10f));
        }

        // Value for smoothly following the player
        currentPos = Vector3.SmoothDamp(cameraT.position, moveTo, ref velocity, movementNumber);

        // Move the camera and make it look at the player
        cameraT.position = currentPos;
        cameraT.LookAt(target, target.up);

        // If not within distance of enemy
        if(changeLookAt == false){
            offset = Vector3.Lerp(offset, defaultOffset, 1.0f - Mathf.Pow(0.5f, Time.deltaTime * 5f));
            //battleMusic.Stop();
            defaultOffset = origOffset;
        }
        // If within distance of enemy
        else if(changeLookAt == true && pushCam == true){
            // Normalize the offset between the camera and the player, multiply to push the camera further away
            offset = offset.normalized * 47;
            defaultOffset = offset;
            pushCam = false;

            //battleMusic.Play();
        }
    }
}
