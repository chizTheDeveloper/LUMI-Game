using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPressed : MonoBehaviour
{

    void Update()
    {
        // Check if enter (return) key has been pressed and go to next sentence in the dialogue set
        if ((Input.GetKeyDown("e") || Input.GetKeyDown("joystick button 0")) && FindObjectOfType<DialogueManager>().dialogueText.enabled == true){
            FindObjectOfType<DialogueManager>().DisplayNext();
        }
    }
}
