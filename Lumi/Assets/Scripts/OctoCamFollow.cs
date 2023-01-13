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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseX += Input.GetAxis("Mouse X");
        mouseY += Input.GetAxis("Mouse Y");
        octoCam.transform.localRotation = Quaternion.Euler(-mouseY * 0.6f, mouseX * 0.6f, 0);
        octoCam.transform.position = player.transform.position;
    }
}
