using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCJelly : MonoBehaviour
{

    public float lookRadius = 80f;

    public Player player;
    Transform target;

    public KeyCode interactKey;

    public bool startQuest;

    public bool talking;

    QuestTracker questTracker;
    
    //public GameObject heart;
    public GameObject jelly1;
    public GameObject jelly2;
    public GameObject jelly3;

    public bool questDone;

    [SerializeField] public TMP_Text jellyText1;
    [SerializeField] public TMP_Text jellyText2;
    [SerializeField] public TMP_Text jellyText3;
    public Image jellyImage;

   //[SerializeField] public TMP_Text PressE;

    public Image PressE;
    public Image PressA;

    public Image jellyQuest;

    public GameObject gameManager;
    public GameObject damageUp;

    private PowerUp PowerUp;
    AudioSource powerUpAudio;
    public GameObject powerUpSoundObj;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;

        startQuest = false;

        //heart.SetActive(false);
        jelly1.SetActive(false);
        jelly2.SetActive(false);
        jelly3.SetActive(false);

        questTracker = player.GetComponent<QuestTracker>();
        questDone = false;

        talking = false;
        PressE.enabled = false;
        PressA.enabled = false;

        PowerUp = player.GetComponent<PowerUp>();
        powerUpAudio = powerUpSoundObj.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        //Vector3 bobbing = new Vector3(transform.position.x, transform.position.y + ((float)Mathf.Sin(Time.time) * 0.06f), transform.position.z);
        //transform.position = bobbing;

        float distance = Vector3.Distance(target.position, transform.position);

         if(distance <= lookRadius)
        {

        if(talking == true){
            PressE.enabled = false;
            PressA.enabled = false;
        }else{

        if(gameManager.GetComponent<KeyorCont>().keyboard == true){
            PressE.enabled = true;
        }else{
            PressA.enabled = true;
        }
        }
           
            if((startQuest == false || questTracker.jellyCaught < 3) && questDone == false){
            if((Input.GetKeyDown("e") || Input.GetKeyDown("joystick button 0")) && talking == false)
            {
                //play here text introducing quest
                questTracker = player.GetComponent<QuestTracker>();
                questTracker.progress.Add("jellyStart");
                questTracker.interactingWith = "npcL1";

                startQuest = true;
            }
            }
            
            if(startQuest == true && questTracker.jellyCaught == 3 && questDone == false){
                if(Input.GetKeyDown("e") || Input.GetKeyDown("joystick button 0"))
                {
                //play here textending quest

                questTracker = player.GetComponent<QuestTracker>();
                questTracker.progress.Add("jellyEnd");
                questTracker.interactingWith = "npcL1_end";

                jelly1.SetActive(true);
                jelly2.SetActive(true);
                jelly3.SetActive(true);

                questDone = true;
                jellyText3.enabled = false;
                jellyImage.enabled = false;
                jellyQuest.enabled = false;
                player.SQ1 = true;
                player.playerLevel++;
                Destroy(GameObject.FindWithTag("questDest"));
                powerUpAudio.Play();
                PressE.enabled = false;

                for(int i = 0; i< PowerUp.slots.Length; i++){
                    if(PowerUp.isFull[i] == false){

                        PowerUp.isFull[i] = true;
                        Instantiate(damageUp, PowerUp.slots[i].transform, false);
                        break;
                    }
                }
                }
            }
        }else{
            PressE.enabled = false;
            PressA.enabled = false;
        }

        if(questDone == true){
            PressE.enabled = false;
            PressA.enabled = false;
        }

        


            if(questTracker.jellyCaught == 1 && questDone == false){
                jellyText1.enabled = true;
                jellyImage.enabled = true;
            }else if(questTracker.jellyCaught == 2 && questDone == false){
                jellyText1.enabled = false;
                jellyText2.enabled = true;
                jellyImage.enabled = true;
            }else if(questTracker.jellyCaught == 3 && questDone == false){
                jellyText2.enabled = false;
                jellyText3.enabled = true;
                jellyImage.enabled = true;
            }
        
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}