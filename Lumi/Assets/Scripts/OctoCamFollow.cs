using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctoCamFollow : MonoBehaviour
{
    public GameObject player;
    public GameObject octoCam;
    public Transform npc;
    float mouseX;
    float mouseY;

    void Update()
    {
        // Get mouse position
        mouseX += Input.GetAxis("Mouse X");
        mouseY += Input.GetAxis("Mouse Y");

        // Change the rotation of the octopus npc focus camera when they move the mouse
        octoCam.transform.localRotation = Quaternion.Euler(-mouseY * 0.6f, mouseX * 0.6f, 0);
        octoCam.transform.position = player.transform.position;
    }
}
