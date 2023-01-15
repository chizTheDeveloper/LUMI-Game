using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPressed : MonoBehaviour
{
    void Update()
    {
        // Check if enter (return) key has been pressed and go to next sentence in the dialogue set
        if (Input.GetKeyDown("return") || Input.GetKeyDown(KeyCode.Return)){
            FindObjectOfType<DialogueManager>().DisplayNext();
        }
    }
}
