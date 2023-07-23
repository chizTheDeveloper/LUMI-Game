using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{

    public float lookRadius = 80f;

    public Player player;
    Transform target;

    public KeyCode interactKey;
    public KeyCode interactKey2;

    public bool startQuest;
    public bool firstkeyPressed;

    public GameObject[] questBlobs;
    QuestTracker2 questTracker;

    int withinRad;

    public bool boolSet;
    public bool talking;

    private Animator animator;
    
    public GameObject gameManager;
    public Image PressE;
    public Image PressA;
    Movement movement;

    public bool questComp;
    [SerializeField] TMP_Text countDown;

    // Start is called before the first frame update
    void Start()
    {

        target = PlayerManager.instance.player.transform;
        movement = player.GetComponent<Movement>();

        questBlobs[1].SetActive(false);
        withinRad = 0;

        for(int i =0; i<questBlobs.Length; i++){
            questBlobs[i].SetActive(false);
        }

        startQuest = false;
        firstkeyPressed = false;

        boolSet = false;
        talking = false;

        animator = gameObject.GetComponent<Animator>();

        questComp = countDown.GetComponent<CountDownTimer>().questComplete;

        PressE.enabled = false;
        PressA.enabled = false;

        questTracker = player.GetComponent<QuestTracker2>();
    }

    // Update is called once per frame
    void Update()
    {


        float distance = Vector3.Distance(target.position, transform.position);

         if(distance <= lookRadius && startQuest == false && questComp == false)
        {
                
            if((Input.GetKeyDown("e") || Input.GetKeyDown("joystick button 0")) && startQuest == false && firstkeyPressed == false && boolSet == false && talking == false && questTracker.isCamOn == false)
            {
    
                questTracker.progress.Add("blobStart");
                questTracker.interactingWith = "npcL2";

                boolSet = true;
                firstkeyPressed = false;
            }
            if((Input.GetKeyDown("e") || Input.GetKeyDown("joystick button 0")) && startQuest == false && firstkeyPressed == true && boolSet == true )
                {
                    for(int i =0; i<questBlobs.Length; i++){
                     questBlobs[i].SetActive(true);
                    }

                    startQuest = true;
                    firstkeyPressed = false;
                    boolSet = false;
                    PressE.enabled = false;
                    PressA.enabled = false;
             }
             if((Input.GetKeyDown("e") || Input.GetKeyDown("joystick button 0")) && startQuest == false && boolSet == true && talking == true)
                {
                    firstkeyPressed = true;
                }


        
        }

        if(distance <= lookRadius && startQuest == false && questComp == false && talking == false && FindObjectOfType<DialogueManager>().dialogueText.enabled == false){   
            if(gameManager.GetComponent<KeyorCont>().keyboard == true){
                PressE.enabled = true;
            }else{
                PressA.enabled = true;
            }
        }
        else if(movement.distancePlayerOcto > 20){
            PressE.enabled = false;
            PressA.enabled = false;
        }

        
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
