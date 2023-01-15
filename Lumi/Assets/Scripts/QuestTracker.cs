using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestTracker : MonoBehaviour
{
    public GameObject npc;
    public GameObject mainCam;
    public GameObject octoCam;

    public bool hasStarted;
    public bool triggerDialogue;
    public int jellyfishCollected;
    public string interactingWith;
    public List<string> progress = new List<string>();

    public Vector3 velocity = Vector3.one;
    public Vector3 currPos;
    Vector3 npcOrigin;

    public Dialogue dialogue;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI cont;

    void Start()
    {
        // Origin location for NPC so he can be sent back to that area when he's not interacting with the player
        npcOrigin = new Vector3(5751.4f, 39, -12435);

        // Coroutine to start krill companion intro
        StartCoroutine(Intro(4));
        
        // Setting some variables that are used throughout progress
        hasStarted = false;
        interactingWith = "";
        triggerDialogue = false;
        jellyfishCollected = 0;

        // Default to main camera
        mainCam.SetActive(true);

        // Hide the dialogue UI
        nameText.enabled = false;
        dialogueText.enabled = false;
        cont.enabled = false;
    }

    void Update()
    {
        // Once octopus has been met and quests have started
        if(hasStarted == true){
            // Create new position vector that is the same as the player, use sin for bobbing up and down effect
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + ((float)Mathf.Sin(Time.time) * 3), transform.position.z);
            
            // Create new location for NPC in front of player based on its direction and the vector created above
            Vector3 newLocation = (transform.forward * 20) + pos;

            // Smooth the movements
            Vector3 smooth = Vector3.SmoothDamp(npc.transform.position, newLocation, ref velocity, 0.3f);
            
            // Apply the change in position
            npc.transform.position = smooth;

            // If dialogue hasn't been triggered yet, start coroutine to swap to the npc camera
            if(triggerDialogue == false){
                // Change which camera is active while npc is talking to you
                StartCoroutine(SwapCameras(2));
                triggerDialogue = true;
            }
        }

        // If player is interacting with the companion npc, use the main camera and trigger companion dialogue
        if(interactingWith == "companion"){
            octoCam.SetActive(false);
            mainCam.SetActive(true);
            TriggerCompDial();
        }
        
        // Cycle through the progress milestones
        foreach(string item in progress)
        {
            // If intro has been passed, trigger the start of the quest system
            if(item.Contains("intro")){
                StartCoroutine(StartQuests(10));
            }
            // If player has finished their first encounter with the octopus npc
            else if(item.Contains("firstNpcEncounter")){
                mainCam.SetActive(true);
                octoCam.SetActive(false);

                // Switch to the main camera and send the octopus npc out of the scene
                Vector3 smoothSendBack = Vector3.SmoothDamp(npc.transform.position, npcOrigin, ref velocity, 0.3f);
                npc.transform.position = smoothSendBack;
            }
        }
    }

    IEnumerator Intro(float time)
    {
        // Wait before starting companion intro dialogue
        yield return new WaitForSeconds(time);
        interactingWith = "companion";
    }

    IEnumerator StartQuests(float time)
    {
        // Set quest system to "has started" to trigger first octopus encounter in Update
        yield return new WaitForSeconds(time);
        hasStarted = true;
    }

    IEnumerator SwapCameras(float time)
    {
        // Set quest system to "has started" to trigger first octopus encounter in Update
        yield return new WaitForSeconds(time);
        
        // Switch to octopus npc camera
        octoCam.SetActive(true);
        mainCam.SetActive(false);

        // Trigger the dialogue
        triggerDialogue = true;
        Trigger();
    }

    // Trigger function for octopus npc dialogue
    public void Trigger(){
        // Show dialogue UI
        nameText.enabled = true;
        dialogueText.enabled = true;
        cont.enabled = true;

        // Start "StartDialogue" function in the dialogue manager, pass who the player is interacting with to the function
        string speakNpc = "npc";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakNpc);
        interactingWith = "";
    }

    public void TriggerCompDial(){
        // Show dialogue UI
        nameText.enabled = true;
        dialogueText.enabled = true;
        cont.enabled = true;

        // Start "StartDialogue" function in the dialogue manager, pass who the player is interacting with to the function
        string speakComp = "krill";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakComp);
        interactingWith = "";
    }
        
}
