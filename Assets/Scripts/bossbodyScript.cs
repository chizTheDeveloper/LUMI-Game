using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossbodyScript : MonoBehaviour
{
    public Transform player;

    void Start()
    {
    }

    void Update()
    {
        Vector3 lookAtPosition = player.position;
        lookAtPosition.y = transform.position.y;
        transform.LookAt(lookAtPosition);

    }

}
