using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFollow : MonoBehaviour
{

    public Vector3 velocity = Vector3.one;
    public Vector3 offset;

    public Transform NPC;
    public Transform target;


    void Start()
    {
        // Offset for camera position
        offset = NPC.position - target.position;

    }

    void Awake(){
        // Get the camera transform
        NPC = transform;
    }

    void LateUpdate()
    {
        Follow();
    }



    void Update()
    {
       
    }
    

     public void Follow(){
        // Position to move the camera
        Vector3 moveTo = target.position + (target.rotation * offset);

        // Use smooth damp to move camera smoothly with velocity
        Vector3 currentPos = Vector3.SmoothDamp(NPC.position, moveTo, ref velocity, 0.2f);

        // Change the position and look at the player
        NPC.position = currentPos;
        NPC.LookAt(target, target.up);
    }
}
