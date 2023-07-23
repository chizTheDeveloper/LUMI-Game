using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleLife : MonoBehaviour
{

    float TimeLeft = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     TimeLeft -= Time.deltaTime;
      if ( TimeLeft < 0 )
      {
          Destroy(this);
      }
    }
}
