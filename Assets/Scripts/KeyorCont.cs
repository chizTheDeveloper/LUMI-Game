using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyorCont : MonoBehaviour
{

    public bool keyboard;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


       void OnDisable()
        {

            PlayerPrefs.SetInt("keyboard", (keyboard ? 1 : 0));

        }


        void OnEnable()
        {

            keyboard = (PlayerPrefs.GetInt("keyboard") != 0);

        }
}
