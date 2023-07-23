using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class QuestTrackerBoss : MonoBehaviour
{
    public Vector3 velocity = Vector3.one;

    public List<string> progress = new List<string>();
    
    public string interactingWith;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI cont;

    public Dialogue dialogue;
    Movement movement;

    public GameObject mainCam;
    public GameObject krakenCam;
    public GameObject orbPower;
    public GameObject kraken;
    public GameObject dust;
    Vector3 orbRiseSpot;
    Vector3 krakenSinkSpot;
    bool sunk;
    bool risen;
    bool growing;
    GameObject whiteScreen;
    Image whiteImg;
    float turningWhite;

    public GameObject[] legs;
    public Animator[] animatorLeg;

    public bool cutscene;

    // Start is called before the first frame update
    void Start()
    {
        progress.Add("bossIntro");
        interactingWith = "krill";

        movement = GetComponent<Movement>();
        mainCam.SetActive(true);
        krakenCam.SetActive(false);

        //legs = GameObject.FindGameObjectsWithTag("KrakenLeg");
        Debug.Log("Legs: " + legs.Length);

        animatorLeg = new Animator[8];

        for(int i = 0; i < legs.Length; i++){
           animatorLeg[i] = legs[i].GetComponent<Animator>();
        }

        whiteScreen = GameObject.FindWithTag("WhiteScreen");
        whiteScreen.SetActive(true);
        whiteImg = whiteScreen.GetComponent<Image>();
        whiteScreen.GetComponent<Image>().color = new Color(whiteImg.color.r, whiteImg.color.g, whiteImg.color.b, 0.0f);

        cutscene = false;
        orbRiseSpot = new Vector3(5762.9f, 139.5f, -12324.6f);
        krakenSinkSpot = new Vector3(5715.896f, 86.4f, -12476.87f);
        sunk = false;
        risen = false;
        growing = false;
        turningWhite = 0f;

        dust.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(progress.LastOrDefault() == "bossIntro" && interactingWith == "krill"){
            TriggerIntro();
            interactingWith = "";
            Debug.Log("test");
        }
        else if(progress.LastOrDefault() == "fight"){
            movement.freezeRotation = false;
            movement.gameBegins = true;
            cont.enabled = false;
            progress.Add("fighting");
        }
        //pausing right after
        else if(progress.LastOrDefault() == "defeat" && interactingWith == "kraken"){
            StartCoroutine(DefeatPause(1f));
            progress.Add("pausing");
            interactingWith = "";
        }
        else if(progress.LastOrDefault() == "orbRise" && orbPower.transform.position.y < orbRiseSpot.y){
            orbPower.transform.position = Vector3.SmoothDamp(orbPower.transform.position, orbRiseSpot, ref velocity, 0.5f);
        }
        //death length
        else if(progress.LastOrDefault() == "krakenDying" && interactingWith == "kraken"){
            float deathAnimationLength = 6.5f;
            interactingWith = "";
            orbPower.SetActive(true);
            StartCoroutine(PlayDeathAnimation(deathAnimationLength));
            Debug.Log("play death animation");
        }
        else if(progress.LastOrDefault() == "dead" && interactingWith == "krill"){
            TriggerEnding();
            interactingWith = "";
        }
        else if(progress.LastOrDefault() == "rollCredits" && growing == false){
            StartCoroutine(Credits(5f));
            growing = true;
            kraken.SetActive(false);
        }
        else if(progress.LastOrDefault() == "sink" && kraken.transform.position.y > krakenSinkSpot.y + 10){
            kraken.transform.position = Vector3.SmoothDamp(kraken.transform.position, krakenSinkSpot, ref velocity, 3f);
            dust.SetActive(true);
        }

        if(growing == true && orbPower.transform.localScale.x < 200 && turningWhite < 100f){
            Vector3 scaleChange = new Vector3(1.2f, 1.2f, 1.2f);
            orbPower.transform.localScale += scaleChange;

            whiteScreen.GetComponent<Image>().color = new Color(whiteImg.color.r, whiteImg.color.g, whiteImg.color.b, turningWhite);
            turningWhite = turningWhite + 0.02f;
        }

        if(orbPower.transform.position.y >= orbRiseSpot.y - 2 && risen == false){
            StartCoroutine(BackToKrill(0f));
            kraken.SetActive(false);
            risen = true;
        }

        if(kraken.transform.position.y < krakenSinkSpot.y + 20 && sunk == false){
            sunk = true;

            for(int i=0; i < animatorLeg.Length; i++){
                animatorLeg[i].enabled = false;
            }

            StartCoroutine(Sinking(0f));
        }

       
    }

    void TriggerIntro(){
        nameText.enabled = true;
        dialogueText.enabled = true;
        cont.enabled = true;

        // Start "StartDialogue" function in the dialogue manager, pass who the player is interacting with to the function
        string speakNpc = "krillBoss";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakNpc);
        interactingWith = "";
    }

    void TriggerEnding(){
        nameText.enabled = true;
        dialogueText.enabled = true;
        cont.enabled = true;
        interactingWith = "";

        // Start "StartDialogue" function in the dialogue manager, pass who the player is interacting with to the function
        string speakNpc = "krillDefeat";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, speakNpc);
    }

    //play animation in here
    IEnumerator DefeatPause(float time)
    {
        yield return new WaitForSeconds(time);
        //mainCam.SetActive(false);
        //krakenCam.SetActive(true);
        progress.Add("krakenDying");
        interactingWith = "kraken";
        cutscene = true;

        for(int i=0; i < animatorLeg.Length; i++){
            //animatorLeg[i].Play("krakenLeg1-V1-DYING", 13, 0);
            animatorLeg[i].SetBool("isDead", true);
        }
    }

    IEnumerator PlayDeathAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        progress.Add("sink");
    }

    IEnumerator Sinking(float time)
    {
        yield return new WaitForSeconds(time);
        progress.Add("orbRise");
    }

    IEnumerator BackToKrill(float time)
    {
        yield return new WaitForSeconds(time);
        progress.Add("dead");
        interactingWith = "krill";
        //cutscene = false;
    }

    IEnumerator Credits(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("CREDITS");
    }
}
