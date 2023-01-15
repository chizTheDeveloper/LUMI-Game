using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishMove : MonoBehaviour
{

    public GameObject player;
    public float distanceBetween;
    float startY;
    Vector3 jellyPos;

    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        jellyPos = transform.position;
        distanceBetween = Vector3.Distance(transform.position, player.transform.position);

        if(distanceBetween < 3.5f){
            Vector3 moveTo = transform.position - player.transform.position;
            //moveTo.y = player.transform.position.y;
            transform.Translate(moveTo * Random.Range(0.05f, 0.3f) * Time.deltaTime);
            //startY = transform.position.y;
        }
        else{
            //transform.position = new Vector3(jellyPos.x, startY + ((float)Mathf.Sin(Time.time) * 0.3f), jellyPos.z);
        }
    }
}
