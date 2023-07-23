using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctoChatBub : MonoBehaviour
{

    public GameObject chatCanvas;
    public GameObject chatIcon;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        chatCanvas.SetActive(false);
        chatIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<Movement>().gameBegins == true && FindObjectOfType<DialogueManager>().dialogueText.enabled == false){
            chatCanvas.SetActive(true);
            chatIcon.SetActive(true);
        }
    }
}
