using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMapCam : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mapCamPos = player.transform.position;
        mapCamPos.y = mapCamPos.y + 2000;

        transform.position = mapCamPos;
    }
}
