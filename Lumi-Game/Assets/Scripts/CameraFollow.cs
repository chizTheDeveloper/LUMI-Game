using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 velocity = Vector3.one;
    public Vector3 offset;
    
    public Transform cameraT;
    public Transform target;

    void Start()
    {
        // Offset for camera position
        offset = cameraT.position - target.position;
    }

    void Awake(){
        // Get the camera transform
        cameraT = transform;
    }

    void LateUpdate()
    {
        Follow();
    }

    public void Follow(){
        // Position to move the camera
        Vector3 moveTo = target.position + (target.rotation * offset);

        // Use smooth damp to move camera smoothly with velocity
        Vector3 currentPos = Vector3.SmoothDamp(cameraT.position, moveTo, ref velocity, 0.3f);

        // Change the position and look at the player
        cameraT.position = currentPos;
        cameraT.LookAt(target, target.up);
    }
}
