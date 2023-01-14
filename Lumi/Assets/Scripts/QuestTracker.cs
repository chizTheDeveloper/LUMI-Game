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

    public Dialogue dialogue;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI cont;

    // Start is called before the first frame update
    void Start()
    {
        // Coroutine for being approached by the octopus npc, set to 5 seconds for testing
        
        StartCoroutine(Intro(5));
        //StartCoroutine(StartQuests(5));
        
        hasStarted = false;
        interactingWith = "companion";
        triggerDialogue = false;
        jellyfishCollected = 0;

        // Default to main camera
        mainCam.SetActive(true);

        nameText.enabled = false;
        dialogueText.enabled = false;
        cont.enabled = false;
    }

    // Update is called once per frame
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

            if(triggerDialogue == false){
                // Change which camera is active while npc is talking to you
                StartCoroutine(SwapCameras(2));
                triggerDialogue = true;
            }
        }

        if(interactingWith == "companion"){
            octoCam.SetActive(false);
            mainCam.SetActive(true);
            Debug.Log("Companion");
            TriggerCompDial();
        }
        
        foreach(string item in progress)
        {
            if(item.Contains("intro")){
                StartCoroutine(StartQuests(5));
            }
            else if(item.Contains("firstNpcEncounter")){
                mainCam.SetActive(true);
                octoCam.SetActive(true);
            }
        }
    }

    IEnumerator Intro(float time)
    {
        // Wait before starting companion dialogue
        yield return new WaitForSeconds(time);
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
        octoCam.SetActive(true);
        mainCam.SetActive(false);
        triggerDialogue = true;
        Trigger();

    }

    public void Trigger(){
        nameText.enabled = true;
        dialogueText.enabled = true;
        cont.enabled = true;
        string speakNpc = "npc";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakNpc);
    }

    public void TriggerCompDial(){
        nameText.enabled = true;
        dialogueText.enabled = true;
        cont.enabled = true;
        string speakComp = "intro";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakComp);
        interactingWith = "";
    }
        
}
