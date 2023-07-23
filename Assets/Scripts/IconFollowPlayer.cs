using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconFollowPlayer : MonoBehaviour
{

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 iconPos = player.transform.position;
        float iconRotation = player.transform.eulerAngles.y;

        transform.position = iconPos;
        transform.eulerAngles = new Vector3(90f,iconRotation,0f);
    }
}
