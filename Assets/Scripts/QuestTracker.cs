using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class QuestTracker : MonoBehaviour
{
    public GameObject npc;
    public GameObject mainCam;
    public GameObject octoCam;
    public GameObject endCam;
    public GameObject[] jelly;
    Movement movement;
    public GameObject playerModel;
    public GameObject coralTrail;
    bool checkedCave;

    public bool hasStarted;
    public int xpFoundCount;
    public bool triggerDialogue;
    public int jellyfishCollected;
    public string interactingWith;
    public bool keyFound;
    public bool jellyHint;
    public int jellyCaught;
    public List<string> progress = new List<string>();

    public Vector3 velocity = Vector3.one;
    public Vector3 currPos;
    Vector3 npcOrigin;
    Vector3 npcSittingSpot;
    Vector3 caveSpot;

    public Dialogue dialogue;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI cont;
    public TextMeshProUGUI contA;

    public bool activity2 = false;
    float goUp;
    bool bobbing;

    public GameObject gameManager;
    bool octoMoveTowards;

    AudioSource keySound;
    public GameObject keySoundObj;

    AudioSource heartSound;
    public GameObject heartSoundObj;

    void Start()
    {
        // Origin location for NPC so he can be sent back to that area when he's not interacting with the player
        npcOrigin = new Vector3(5676.402f, 42.7f, -12269.67f);
        npcSittingSpot = new Vector3(5667.3f, 38.5f, -12118);
        caveSpot = new Vector3(6468.9f, 35.4f, -11507.1f);

        bobbing = false;
        xpFoundCount = 0;
        goUp = -30f;
        checkedCave = false;

        movement = GetComponent<Movement>();

        // Coroutine to start krill companion intro
        StartCoroutine(Intro(0.5f));
        
        // Setting some variables that are used throughout progress
        hasStarted = false;
        interactingWith = "";
        triggerDialogue = false;
        keyFound = false;
        jellyfishCollected = 0;
        jellyCaught = 0;
        jellyHint = false;

        // Default to main camera
        mainCam.SetActive(true);

        // Hide the dialogue UI
        nameText.enabled = false;
        dialogueText.enabled = false;
        cont.enabled = false;
        contA.enabled = false;

        coralTrail = GameObject.FindWithTag("CoralTrail");
        coralTrail.SetActive(false);

        octoMoveTowards = true;

        keySound = keySoundObj.GetComponent<AudioSource>();
        heartSound = heartSoundObj.GetComponent<AudioSource>();
    }

    void Update()
    {
        // Once octopus has been met and quests have started
        if(hasStarted == true){
            // Create new position vector that is the same as the player, use sin for bobbing up and down effect
            Vector3 pos = new Vector3(transform.position.x, transform.position.y/* + ((float)Mathf.Sin(Time.time) * 3)*/, transform.position.z);
            
            // Create new location for NPC in front of player based on its direction and the vector created above
            Vector3 newLocation = (transform.forward * 20) + pos;

            // Smooth the movements
            Vector3 smooth = Vector3.SmoothDamp(npc.transform.position, newLocation, ref velocity, 1.25f);
            
            // Apply the change in position
            npc.transform.position = smooth;

            // If dialogue hasn't been triggered yet, start coroutine to swap to the npc camera
            if(triggerDialogue == false){
                // Change which camera is active while npc is talking to you
                StartCoroutine(SwapCameras(2));
                triggerDialogue = true;
            }

            Debug.Log("rotating");
        }

        if(bobbing == true){
            Vector3 pos = new Vector3(npc.transform.position.x, npc.transform.position.y + ((float)Mathf.Sin(Time.time) * 2), npc.transform.position.z);
            Vector3 smooth = Vector3.SmoothDamp(npc.transform.position, pos, ref velocity, 1.25f);
            npc.transform.position = smooth;
        }

        // If player is interacting with the companion npc, use the main camera and trigger companion dialogue
        if(interactingWith == "companion" && !progress.Contains("intro")){
            octoCam.SetActive(false);
            mainCam.SetActive(true);
            TriggerCompDial();
        }

        if(progress.LastOrDefault() == "jellyStart" && interactingWith == "npcL1"){
            TriggerNPCDial();
            interactingWith = "";
        }

        
        if(progress.LastOrDefault() == "jellyEnd" && interactingWith == "npcL1_end"){
            TriggerNPCDialTwo();
            interactingWith = "";
        }
        
            // If intro has been passed, trigger the start of the quest system
            if(progress.LastOrDefault() == "intro"){
                StartCoroutine(StartQuests(0.1f));
                progress.Add("onlyQuestOnce");
                jellyHint = true;
            }
            // If player has finished their first encounter with the octopus npc
            else if(progress.LastOrDefault() == "firstNpcEncounter"){
                mainCam.SetActive(true);
                octoCam.SetActive(false);
                playerModel.SetActive(true);
                movement.freezeRotation = false;
                movement.gameBegins = true;
                octoMoveTowards = false;

                // Switch to the main camera and send the octopus npc out of the scene
                Vector3 smoothSendBack = Vector3.SmoothDamp(npc.transform.position, npcSittingSpot, ref velocity, 1.0f);
                npc.transform.position = smoothSendBack;
                hasStarted = false;
                interactingWith = "";
                jellyHint = false;

            }
            // COMBINE THESE
            else if(progress.LastOrDefault() == "nearSecretArea" && interactingWith == "companion" && keyFound == false){
                TriggerCompDial();
                interactingWith = "";
            }
            else if(progress.LastOrDefault() == "pickedUpKey" && interactingWith == "companion"){
                TriggerCompDial();
                interactingWith = "";
                keySound.Play();
            }
            else if(progress.LastOrDefault() == "triedToExit" && interactingWith == "krill" && checkedCave == false){
                TriggerCompDial();
                interactingWith = "";
                checkedCave = true;
            }
            else if(progress.LastOrDefault() == "foundHeart" && interactingWith == "companion"){
                TriggerCompDial();
                heartSound.Play();
                interactingWith = "";
            }
            else if(progress.LastOrDefault() == "jellyTip" && interactingWith == "npc"){
                jellyHint = true;
                Trigger();
                interactingWith = "";
            }
            else if(progress.LastOrDefault() == "octoTip" && interactingWith == "npc"){
                Trigger();
                interactingWith = "";
            }
            else if(progress.Contains("congratsJelly") && interactingWith == "npc"){
                Trigger();
                interactingWith = "";
            }
            else if(progress.LastOrDefault() == "foundOrb"){
                //Time.timeScale = 0f;
                //activity2 = true;
                progress.Add("complete");
                interactingWith = "npc";
            }
            else if(progress.LastOrDefault() == "complete" && interactingWith == "npc"){
                interactingWith = "";
                movement.hintCount = 1;

                StartCoroutine(FoundOrbL1(1.5f));
            }
            else if(progress.LastOrDefault() == "nextLevel"){
                //Time.timeScale = 1f;
                //activity2 = false;
                /*
                Vector3 smoothSendBack = Vector3.SmoothDamp(npc.transform.position, caveSpot, ref velocity, 5f);
                npc.transform.position = smoothSendBack;
                */
                //hasStarted = false;

                mainCam.SetActive(true);
                endCam.SetActive(false);

                StartCoroutine(CoralRisePause(2f));
            }
            else if(progress.LastOrDefault() == "riseCorals"){
                if(coralTrail.transform.position.y < 0){
                    coralTrail.transform.position = new Vector3(coralTrail.transform.position.x, goUp, coralTrail.transform.position.z);
                    goUp = goUp + 1f;
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

    IEnumerator CoralRisePause(float time)
    {
        // Set quest system to "has started" to trigger first octopus encounter in Update
        yield return new WaitForSeconds(time);
        progress.Add("riseCorals");
        bobbing = false;
    }

    IEnumerator FoundOrbL1(float time)
    {
        // Set quest system to "has started" to trigger first octopus encounter in Update
        yield return new WaitForSeconds(time);
        octoFollow();
        Trigger();
        coralTrail.SetActive(true);
        mainCam.SetActive(false);
        endCam.SetActive(true);
        bobbing = true;
    }

    IEnumerator SwapCameras(float time)
    {
        // Set quest system to "has started" to trigger first octopus encounter in Update
        yield return new WaitForSeconds(time);
        
        // Switch to octopus npc camera
        octoCam.SetActive(true);
        mainCam.SetActive(false);
        playerModel.SetActive(false);

        // Trigger the dialogue
        triggerDialogue = true;
        movement.freezeRotation = true;
        Trigger();
    }

    // Trigger function for octopus npc dialogue
    public void Trigger(){
        // Show dialogue UI
        nameText.enabled = true;
        dialogueText.enabled = true;
        if(gameManager.GetComponent<KeyorCont>().keyboard == true){
            cont.enabled = true;
        }else{
            contA.enabled = true;
        }

        // Start "StartDialogue" function in the dialogue manager, pass who the player is interacting with to the function
        string speakNpc = "npc";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakNpc);
        interactingWith = "";
    }

    public void TriggerCompDial(){
        // Show dialogue UI
        nameText.enabled = true;
        dialogueText.enabled = true;
        if(gameManager.GetComponent<KeyorCont>().keyboard == true){
            cont.enabled = true;
        }else{
            contA.enabled = true;
        }

        // Start "StartDialogue" function in the dialogue manager, pass who the player is interacting with to the function
        string speakComp = "krill";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakComp);
        interactingWith = "";
    }

     public void TriggerNPCDial(){
        // Show dialogue UI
        nameText.enabled = true;
        dialogueText.enabled = true;
        if(gameManager.GetComponent<KeyorCont>().keyboard == true){
            cont.enabled = true;
        }else{
            contA.enabled = true;
        }

        // Start "StartDialogue" function in the dialogue manager, pass who the player is interacting with to the function
        string speakL1 = "npcL1";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakL1);
        interactingWith = "";
    }

    public void TriggerNPCDialTwo(){
        // Show dialogue UI
        nameText.enabled = true;
        dialogueText.enabled = true;
        if(gameManager.GetComponent<KeyorCont>().keyboard == true){
            cont.enabled = true;
        }else{
            contA.enabled = true;
        }

        // Start "StartDialogue" function in the dialogue manager, pass who the player is interacting with to the function
        string speakL1_end = "npcL1_end";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakL1_end);
        interactingWith = "";
    }

    public void octoFollow(){
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + ((float)Mathf.Sin(Time.time) * 3), transform.position.z);
            
        // Create new location for NPC in front of player based on its direction and the vector created above
        Vector3 newLocation = new Vector3(6506.7f, 23.2f, -12054.7f);

        // Smooth the movements
        //Vector3 smooth = Vector3.SmoothDamp(npc.transform.position, newLocation, ref velocity, 0.3f);
                
        // Apply the change in position
        npc.transform.position = newLocation;
        npc.transform.rotation = Quaternion.Euler(0, 258.43f, 0);
    }
}
