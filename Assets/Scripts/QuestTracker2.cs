using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class QuestTracker2 : MonoBehaviour
{
    public GameObject npc;
    public GameObject mainCam;
    public GameObject endCam;
    public GameObject NPCCam;
    public GameObject octoCam;
    public GameObject OctoNPC;
    Movement movement;
    public GameObject playerModel;
    public GameObject coralTrail2;
    bool checkedCave;

    public int xpFoundCount;
    public bool triggerDialogue;
    public string interactingWith;
    public List<string> progress = new List<string>();
    float distanceSendSpot;
    float goUp;

    public Vector3 velocity = Vector3.one;
    public Vector3 currPos;
    Vector3 sendSpot;
    Vector3 npcOrigin;
    Vector3 npcSittingSpot;
    Vector3 caveSpot;
    bool bobbing;

    public Dialogue dialogue;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI cont;
    public TextMeshProUGUI contA;
    public GameObject gameManager;

    public bool onNPC;
    int hintCount;
    float distancePlayerOcto;

    public bool isCamOn;

    AudioSource powerUpAudio;
    public GameObject powerUpSoundObj;

    AudioSource heartSound;
    public GameObject heartSoundObj;

    void Start()
    {
        // Origin location for NPC so he can be sent back to that area when he's not interacting with the player
        npcOrigin = new Vector3(6056, 540.6f, -12019);
        npcSittingSpot = new Vector3(5667.3f, 38.5f, -12118);
        sendSpot = new Vector3(4656f, 67.4f, 4428.5f);
        caveSpot = new Vector3(948.6f, 34.9f, 2220.3f);

        xpFoundCount = 0;
        bobbing = false;
        goUp = -35f;
        movement = GetComponent<Movement>();
        triggerDialogue = false;
        checkedCave = false;

        // Default to main camera
        mainCam.SetActive(true);
        NPCCam.SetActive(false);
        isCamOn = false;

        hintCount = 0;

        // Hide the dialogue UI
        nameText.enabled = false;
        dialogueText.enabled = false;
        cont.enabled = false;
        contA.enabled = false;
        progress.Add("L2intro");
        interactingWith = "krillL2";
        movement.gameBegins = true;

        onNPC = false;
        coralTrail2 = GameObject.FindWithTag("CoralTrail");
        coralTrail2.SetActive(false);

        powerUpAudio = powerUpSoundObj.GetComponent<AudioSource>();
        heartSound = heartSoundObj.GetComponent<AudioSource>();
    }

    void Update()
    {
        if(bobbing == true){
            Vector3 pos = new Vector3(npc.transform.position.x, npc.transform.position.y + ((float)Mathf.Sin(Time.time) * 2), npc.transform.position.z);
            Vector3 smooth = Vector3.SmoothDamp(npc.transform.position, pos, ref velocity, 1.25f);
            npc.transform.position = smooth;
        }

        movement.gameBegins = true;

        distancePlayerOcto = Vector3.Distance(transform.position, OctoNPC.transform.position);

        /*if(distancePlayerOcto < 20 && hintCount == 0){
            if((Input.GetKeyDown("e") || Input.GetKeyDown("joystick button 0"))){
            progress.Add("octoTipL2");
            interactingWith = "octoL2";
            hintCount = 1;
            }
        }
        */
        
        if(progress.LastOrDefault() == "blobStart" && interactingWith == "npcL2"){
            TriggerNPCDial();
            interactingWith = "";
        }
        else if(progress.LastOrDefault() == "L2intro" && interactingWith == "krillL2"){
            TriggerCompDial();
            interactingWith = "";
        }
        else if(progress.LastOrDefault() =="blobFail" && interactingWith == "npcL2_Fail"){
            StartCoroutine(SwapCameras(0));
            interactingWith = "";
            onNPC=true;
        }
        else if(progress.LastOrDefault() == "blobPass" && interactingWith == "npcL2_Pass"){
            StartCoroutine(SwapCameras2(0));
            interactingWith = "";
        }
        else if(progress.LastOrDefault() == "octoTipL2" && interactingWith == "octoL2"){
            TriggerOct();
            interactingWith = "";
        }
        else if(progress.LastOrDefault() == "triedToExit" && interactingWith == "krill" && checkedCave == false){
            TriggerCompDial();
            interactingWith = "";
            checkedCave = true;
        }
        else if(progress.LastOrDefault() == "failedBlob"){
            mainCam.SetActive(true);
            NPCCam.SetActive(false);
            isCamOn = false;
            playerModel.SetActive(true);
            interactingWith = "";
            progress.Add("continueL2");
            Debug.Log("failed");
        }
        else if(progress.LastOrDefault() == "passedBlob"){
            mainCam.SetActive(true);
            NPCCam.SetActive(false);
            isCamOn = false;
            playerModel.SetActive(true);
            interactingWith = "";
            progress.Add("continueL2");
            Debug.Log("pass");
        }
        else if(progress.LastOrDefault() == "secondOrbFound" && interactingWith == "octoL2"){
            npc.transform.position = caveSpot;
            npc.transform.rotation = Quaternion.Euler(0, 155.711f, 0);

            StartCoroutine(FoundOrbL2(1.5f));
            interactingWith = "";
        }
        else if(progress.LastOrDefault() == "foundHeart" && interactingWith == "companion"){
            heartSound.Play();
            interactingWith = "";
        }
        else if(progress.LastOrDefault() == "nextLevel2"){
            mainCam.SetActive(true);
            endCam.SetActive(false);
            npc.transform.position = caveSpot;

            StartCoroutine(CoralRisePause(1.5f));
        }
        else if(progress.LastOrDefault() == "riseCorals"){
            if(coralTrail2.transform.position.y < 0){
                coralTrail2.transform.position = new Vector3(coralTrail2.transform.position.x, goUp, coralTrail2.transform.position.z);
                goUp = goUp + 1f;
            }
        }
        
    
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

    public void TriggerOct(){
        // Show dialogue UI
        nameText.enabled = true;
        dialogueText.enabled = true;
        if(gameManager.GetComponent<KeyorCont>().keyboard == true){
            cont.enabled = true;
        }else{
            contA.enabled = true;
        }
        string speakNpc = "octoL2";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakNpc);
        interactingWith = "";
    }

    IEnumerator FoundOrbL2(float time)
    {
        // Set quest system to "has started" to trigger first octopus encounter in Update
        yield return new WaitForSeconds(time);
        coralTrail2.SetActive(true);
        mainCam.SetActive(false);
        endCam.SetActive(true);
        bobbing = true;
        TriggerOct();
    }

    IEnumerator CoralRisePause(float time)
    {
        // Set quest system to "has started" to trigger first octopus encounter in Update
        yield return new WaitForSeconds(time);
        progress.Add("riseCorals");
        bobbing = false;
    }

    IEnumerator SwapCameras(float time)
    {
        // Set quest system to "has started" to trigger first octopus encounter in Update
        yield return new WaitForSeconds(time);
        
        // Switch to octopus npc camera
        NPCCam.SetActive(true);
        mainCam.SetActive(false);
        isCamOn = true;

        // Trigger the dialogue
        triggerDialogue = true;
        TriggerFailDial();
        onNPC=false;
    }

    IEnumerator SwapCameras2(float time)
    {
        // Set quest system to "has started" to trigger first octopus encounter in Update
        yield return new WaitForSeconds(time);

        powerUpAudio.Play();
        
        // Switch to octopus npc camera
        NPCCam.SetActive(true);
        isCamOn = true;
        mainCam.SetActive(false);
        octoCam.SetActive(false);

        // Trigger the dialogue
        triggerDialogue = true;
        TriggerPassDial();
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
        string speakL2 = "npcL2";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakL2);
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
        string speakComp = "krillL2";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakComp);
        interactingWith = "";
    }

    public void TriggerFailDial(){
        // Show dialogue UI
        nameText.enabled = true;
        dialogueText.enabled = true;
        if(gameManager.GetComponent<KeyorCont>().keyboard == true){
            cont.enabled = true;
        }else{
            contA.enabled = true;
        }

        // Start "StartDialogue" function in the dialogue manager, pass who the player is interacting with to the function
        string speakL2_Fail = "npcL2_Fail";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakL2_Fail);
        interactingWith = "";
    }

    public void TriggerPassDial(){
        // Show dialogue UI
        nameText.enabled = true;
        dialogueText.enabled = true;
        if(gameManager.GetComponent<KeyorCont>().keyboard == true){
            cont.enabled = true;
        }else{
            contA.enabled = true;
        }

        // Start "StartDialogue" function in the dialogue manager, pass who the player is interacting with to the function
        string speakL2_Pass = "npcL2_Pass";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakL2_Pass);
        interactingWith = "";
    }

    public void octoFollow(){
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + ((float)Mathf.Sin(Time.time) * 3), transform.position.z);
        Vector3 newLocation = new Vector3(1836f, 33.1f, 1176.4f);
        npc.transform.position = newLocation;
    }
}
