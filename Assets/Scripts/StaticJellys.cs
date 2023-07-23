using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticJellys : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 bobbing = new Vector3(transform.position.x, transform.position.y + ((float)Mathf.Sin(Time.time) * 0.04f), transform.position.z);
        transform.position = bobbing;
    }
}
