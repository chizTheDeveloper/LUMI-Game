using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountDownTimer : MonoBehaviour
{

    float currentTime = 0f;

    float startingTime = 30f;

    [SerializeField] TMP_Text countDown;

    public NPC npc;

    private Animator animator;

    public bool questComplete;

    public int collected;

    public Player player;

    public Image crabQuest;

    QuestTracker2 questTracker;

    public GameObject fartherBlub;

    private PowerUp PowerUp;

    // Start is called before the first frame update
    void Start()
    {
        PowerUp = player.GetComponent<PowerUp>();

        currentTime = startingTime;

        countDown.enabled = false;

        npc = GameObject.FindGameObjectWithTag("NPC").GetComponent<NPC>();

        collected = 0;

        questComplete = false;

        animator = npc.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if(questComplete == false){
            if(npc.startQuest == true){
                countDown.enabled = true;
                currentTime -= 1 * Time.deltaTime;
                countDown.text = currentTime.ToString("0");

                if(currentTime <= 0){
                    currentTime = 30;
                    countDown.enabled = false;

                    questTracker = player.GetComponent<QuestTracker2>();
                    questTracker.progress.Add("blobFail");
                    questTracker.interactingWith = "npcL2_Fail";

                    for(int i =0; i<npc.questBlobs.Length; i++){
                    npc.questBlobs[i].SetActive(false);
                    }
                    collected = 0;

                    npc.firstkeyPressed = false;
                    npc.startQuest = false;
                }
             }
        }else{
            countDown.enabled = false;
            for(int i =0; i<npc.questBlobs.Length; i++){
                    npc.questBlobs[i].SetActive(false);
                    }

            npc.startQuest = true;
        }

        if(collected == 4){
            questComplete = true;
            countDown.enabled = false;
            for(int i =0; i<npc.questBlobs.Length; i++){
                    npc.questBlobs[i].SetActive(false);
                    }
            collected = 0;
            player.SQ2 = true;
            player.playerLevel++;
            crabQuest.enabled = false;
            Destroy(GameObject.FindWithTag("questDest"));

            questTracker = player.GetComponent<QuestTracker2>();
            questTracker.progress.Add("blobPass");
            questTracker.interactingWith = "npcL2_Pass";
            npc.firstkeyPressed = false;
            npc.startQuest = true;
            animator.SetBool("questDone", true);

            for(int i = 0; i< PowerUp.slots.Length; i++){
                    if(PowerUp.isFull[i] == false){

                        PowerUp.isFull[i] = true;
                        Instantiate(fartherBlub, PowerUp.slots[i].transform, false);
                        break;
                    }
                }
        }
    }
}
