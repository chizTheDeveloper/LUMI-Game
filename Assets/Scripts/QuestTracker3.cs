using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class QuestTracker3 : MonoBehaviour
{
    public GameObject npc;
    public GameObject mainCam;
    public GameObject NPCCam;
    public GameObject endCam;
    public GameObject racingFish;
    Movement movement;
    public GameObject playerModel;
    public GameObject coralTrail3;
    public GameObject raceReturnSpot;
    bool checkedCave;

    public int xpFoundCount;
    public bool triggerDialogue;
    public string interactingWith;
    public bool keyFound;
    public bool haveStartedRace;
    public List<string> progress = new List<string>();
    float distanceRacingFish;
    float distanceSendSpot;

    public GameObject raceToRock;

    public Vector3 velocity = Vector3.one;
    public Vector3 currPos;
    Vector3 sendSpot;
    Vector3 npcOrigin;
    Vector3 npcSittingSpot;

    public Dialogue dialogue;
    bool removeEnemies;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI cont;
    public TextMeshProUGUI contA;

    public KeyCode interactKey;

    public GameObject gameManager;
    public Image PressE;
    public Image PressA;

    public KeyCode interactKey9;

    public GameObject showOrb1;
    public GameObject showOrb2;
    public GameObject showOrb3;
    int orbPause;

    public bool firstKeyPressed;
    public bool secondKeyPressed;
    bool onNPC = false;

    public bool startQuest;

    float riseY;
    float goUp;
    bool bobbing;
    GameObject raceTrack;
    Vector3 directionVect;
    private Quaternion lookAtRock;
    
    public GameObject timer;
    GameObject[] enemies;

    AudioSource powerUpAudio;
    public GameObject powerUpSoundObj;

    AudioSource heartSound;
    public GameObject heartSoundObj;

    void Start()
    {
        // Origin location for NPC so he can be sent back to that area when he's not interacting with the player
        npcOrigin = new Vector3(6056, 540.6f, -12019);
        npcSittingSpot = new Vector3(5667.3f, 38.5f, -12118);
        sendSpot = new Vector3(4593.3f, 57f, 5233.8f);

        xpFoundCount = 0;
        haveStartedRace = false;
        bobbing = false;

        showOrb1.SetActive(false);;
        showOrb2.SetActive(false);
        showOrb3.SetActive(false);
        orbPause = 0;
        removeEnemies = false;

        enemies = GameObject.FindGameObjectsWithTag("EnemyZoom");

        riseY = 67.74358f;
        raceTrack = GameObject.Find("racetrackcorals");
        goUp = 0;

        movement = GetComponent<Movement>();
        movement.gameBegins = true;
        checkedCave = false;
        
        // Setting some variables that are used throughout progress
        triggerDialogue = false;
        keyFound = false;

        // Default to main camera
        mainCam.SetActive(true);
        endCam.SetActive(false);

        // Hide the dialogue UI
        nameText.enabled = false;
        dialogueText.enabled = false;
        cont.enabled = false;
        contA.enabled = false;

        progress.Add("L3intro");
        interactingWith = "krillL3";

        coralTrail3 = GameObject.FindWithTag("CoralTrail");
        coralTrail3.SetActive(false);

        firstKeyPressed = false;
        secondKeyPressed = false;
        startQuest = false;

        NPCCam.SetActive(false);

        PressE.enabled = false;
        PressA.enabled = false;

        powerUpAudio = powerUpSoundObj.GetComponent<AudioSource>();
        heartSound = heartSoundObj.GetComponent<AudioSource>();
    }

    void Update()
    {
        distanceRacingFish = Vector3.Distance(transform.position, racingFish.transform.position);
        distanceSendSpot = Vector3.Distance(sendSpot, racingFish.transform.position);

        if(progress.LastOrDefault() == "startRace" && raceTrack.transform.position.y < riseY){
            raceTrack.transform.position = new Vector3(raceTrack.transform.position.x, goUp, raceTrack.transform.position.z);
            goUp = goUp + 1f;
        }

        if(bobbing == true){
            Vector3 bobPos = new Vector3(npc.transform.position.x, npc.transform.position.y + ((float)Mathf.Sin(Time.time) * 2), npc.transform.position.z);
            Vector3 bobSmooth = Vector3.SmoothDamp(npc.transform.position, bobPos, ref velocity, 1.25f);
            npc.transform.position = bobSmooth;
        }

        if(removeEnemies == true){
            for (int i = 0; i < enemies.Length; i++){
                enemies[i].SetActive(false);
            }
        }

        if(distanceRacingFish < 35 && haveStartedRace == false){

            if(startQuest == true){
            PressE.enabled = false;
            PressA.enabled = false;
            }else{
                if(gameManager.GetComponent<KeyorCont>().keyboard == true){
                    PressE.enabled = true;
                }else{
                    PressA.enabled = true;
                }
            }

            if((Input.GetKeyDown("e") || Input.GetKeyDown("joystick button 0")) && startQuest == false){
                progress.Add("preRace");
                interactingWith = "fish";
                PressE.enabled = false;
                startQuest = true;
            }


        }else{
            PressE.enabled = false;
            PressA.enabled = false;
        }

        /*if(progress.LastOrDefault() == "preRace"){
            directionVect = (raceToRock.transform.position - transform.position);
            lookAtRock = Quaternion.LookRotation(directionVect);
            Debug.Log(directionVect);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRock, Time.deltaTime * 50f);
        }*/

        if(progress.LastOrDefault() == "sendBack" && distanceSendSpot < 10){
            progress.Add("preRace");
            Debug.Log("Returned");
            startQuest = false;
            haveStartedRace = false;
            racingFish.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if(progress.LastOrDefault() == "preRace" && interactingWith == "fish"){
            movement.freezeRotation = true;
            StartCoroutine(SwapCameras(0));
            interactingWith = "";
            onNPC = true;
        }
        else if(progress.LastOrDefault() == "finishedIntroL3"){
            movement.gameBegins = true;
        }
        else if(progress.LastOrDefault() == "raceTime"){
            progress.Add("startRace");
            NPCCam.SetActive(false);
            mainCam.SetActive(true);
            haveStartedRace = true;
            //movement.pause = 0;
            //transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(progress.LastOrDefault() == "lostRace" && interactingWith == "fish"){
            timer.GetComponent<CountDownTimer2>().currentTime = 3f;
            haveStartedRace = false;
            TriggerFishDial();
            interactingWith = "";
        }
        else if(progress.LastOrDefault() == "foundHeart" && interactingWith == "companion"){
                heartSound.Play();
                interactingWith = "";
        }
        else if(progress.LastOrDefault() == "wonRace" && interactingWith == "fish"){
            haveStartedRace = false;
            TriggerFishDial();
            interactingWith = "";
            powerUpAudio.Play();
        }
        else if(progress.LastOrDefault() == "triedToExit" && interactingWith == "krill"  && checkedCave == false){
            TriggerCompDial();
            interactingWith = "";
            checkedCave = true;
        }
        else if(progress.LastOrDefault() == "sendBack"){
            Vector3 smoothSendBack = Vector3.SmoothDamp(racingFish.transform.position, sendSpot, ref velocity, 2.2f);
            racingFish.transform.position = smoothSendBack;
            racingFish.transform.LookAt(raceReturnSpot.transform.position, transform.up);
        }
        else if(progress.LastOrDefault() == "L3intro" && interactingWith == "krillL3"){
            TriggerCompDial();
            interactingWith = "";
        }
        else if(progress.LastOrDefault() == "allOrbsFound" && interactingWith == "krillL3end"){
            TriggerCompDial2();
            interactingWith = "";

            removeEnemies = true;
        }
        else if(progress.LastOrDefault() == "octoBetrays" && interactingWith == "octoL3"){
            //octoFollow();
            interactingWith = "";
            goUp = -30f;
            
            StartCoroutine(FoundOrbL3(0.75f));
        }
        else if(progress.LastOrDefault() == "completeL3"){
            mainCam.SetActive(true);
            endCam.SetActive(false);

            StartCoroutine(CoralRisePause(1.5f));
        }
        else if(progress.LastOrDefault() == "riseCorals"){
            if(coralTrail3.transform.position.y < 0){
                coralTrail3.transform.position = new Vector3(coralTrail3.transform.position.x, goUp, coralTrail3.transform.position.z);
                goUp = goUp + 1f;
            }
        }
        else if(progress.LastOrDefault() == "shrimpGoodLuck"  && interactingWith == "shrimpGL"){
            mainCam.SetActive(true);
            endCam.SetActive(false);

            showOrb1.SetActive(false);
            showOrb2.SetActive(false);
            showOrb3.SetActive(false);

            TriggerCompDial3();
            interactingWith = "";
        }
        else if(progress.LastOrDefault() == "doneL3"){
            octoToCave();
            
            if(coralTrail3.transform.position.y < 0){
                coralTrail3.transform.position = new Vector3(coralTrail3.transform.position.x, goUp, coralTrail3.transform.position.z);
                goUp = goUp + 1f;
            }
        }
        else if(progress.LastOrDefault() == "showOrbs"){
            orbPause++;

            if(orbPause > 300){
                showOrb3.SetActive(true);
                progress.Add("orbsAreVisible");
            }
            else if(orbPause > 200){
                showOrb1.SetActive(true);
            }
            else if(orbPause > 100){
                showOrb2.SetActive(true);
            }
        }
        else if(progress.LastOrDefault() == "octoTipL3" && interactingWith == "octoL3"){
            TriggerTip();
            interactingWith = "";
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
        string speakNpc = "octoL3";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakNpc);
        interactingWith = "";
    }

    public void TriggerTip(){
        // Show dialogue UI
        nameText.enabled = true;
        dialogueText.enabled = true;
        if(gameManager.GetComponent<KeyorCont>().keyboard == true){
            cont.enabled = true;
        }else{
            contA.enabled = true;
        }
        string speakNpc = "octoL3";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakNpc);
        interactingWith = "";
    }

    public void TriggerBetray(){
        // Show dialogue UI
        nameText.enabled = true;
        dialogueText.enabled = true;
        if(gameManager.GetComponent<KeyorCont>().keyboard == true){
            cont.enabled = true;
        }else{
            contA.enabled = true;
        }

        // Start "StartDialogue" function in the dialogue manager, pass who the player is interacting with to the function
        string speakNpc = "octoL3Betray";
        progress.Add("octoL3BetrayMessage");
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakNpc);
    }

    public void TriggerFishDial(){
        // Show dialogue UI
        nameText.enabled = true;
        dialogueText.enabled = true;
        if(gameManager.GetComponent<KeyorCont>().keyboard == true){
            cont.enabled = true;
        }else{
            contA.enabled = true;
        }

        // Start "StartDialogue" function in the dialogue manager, pass who the player is interacting with to the function
        string speakFish = "racingFish";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakFish);
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
        string speakComp = "krillL3";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakComp);
        interactingWith = "";
    }

    public void TriggerCompDial2(){
        // Show dialogue UI
        nameText.enabled = true;
        dialogueText.enabled = true;
        if(gameManager.GetComponent<KeyorCont>().keyboard == true){
            cont.enabled = true;
        }else{
            contA.enabled = true;
        }

        // Start "StartDialogue" function in the dialogue manager, pass who the player is interacting with to the function
        string speakComp = "krillL3end";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakComp);
        interactingWith = "";
    }
    public void TriggerCompDial3(){
        // Show dialogue UI
        nameText.enabled = true;
        dialogueText.enabled = true;
        if(gameManager.GetComponent<KeyorCont>().keyboard == true){
            cont.enabled = true;
        }else{
            contA.enabled = true;
        }

        // Start "StartDialogue" function in the dialogue manager, pass who the player is interacting with to the function
        string speakComp = "krillL3end2";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakComp);
        interactingWith = "";
    }

    public void octoFollow(){
        Vector3 newLocation = new Vector3(4501.4f, 85.5f, 7667.5f);
        npc.transform.position = newLocation;
    }

    void StopTimer()
    {
        haveStartedRace = false;
    }

    void octoToCave(){
        Vector3 sendToCave = new Vector3(2964.828f, 62f, 6984f);

        // Smooth the movements
        Vector3 smooth = Vector3.SmoothDamp(npc.transform.position, sendToCave, ref velocity, 3f);
                
        // Apply the change in position
        npc.transform.position = smooth;
    }

    IEnumerator FoundOrbL3(float time)
    {
        // Set quest system to "has started" to trigger first octopus encounter in Update
        yield return new WaitForSeconds(time);
        coralTrail3.SetActive(true);

        Vector3 newLocation = new Vector3(4528.30664f, 110.199997f, 7677.3999f);
        npc.transform.position = newLocation;
        npc.transform.rotation = Quaternion.Euler(0, 184.377f, 0);
        
        mainCam.SetActive(false);
        endCam.SetActive(true);
        bobbing = true;
        TriggerBetray();
        interactingWith = "";
    }

    IEnumerator CoralRisePause(float time)
    {
        // Set quest system to "has started" to trigger first octopus encounter in Update
        yield return new WaitForSeconds(time);
        progress.Add("riseCorals");
        bobbing = false;
        progress.Add("shrimpGoodLuck");
    }

    IEnumerator SwapCameras(float time)
    {
        yield return new WaitForSeconds(time);

        NPCCam.SetActive(true);
        mainCam.SetActive(false);
        
        transform.position = new Vector3(4563.4f, 57f, 5263.8f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        
        TriggerFishDial();
        interactingWith = "";
    }
}
