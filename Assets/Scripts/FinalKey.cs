using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalKey : MonoBehaviour
{

    public GameObject gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameManager.GetComponent<PauseMenu>().activity){
        Vector3 bobbing = new Vector3(transform.position.x, transform.position.y + ((float)Mathf.Sin(Time.time) * 0.08f), transform.position.z);
        transform.position = bobbing;

        transform.Rotate(0f, 1f, 0f, Space.Self);
         }
    }
}
