using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountDownTimer2 : MonoBehaviour
{

    public float currentTime = 0f;

    float startingTime = 3f;

    [SerializeField] TMP_Text countDown;

    public bool questComplete;
     public bool talked;

    public Player player;

    QuestTracker3 questTracker;

    // Start is called before the first frame update
    void Start()
    {
        questTracker = player.GetComponent<QuestTracker3>();

        currentTime = startingTime;

        countDown.enabled = false;

        questComplete = false;

        talked = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(questComplete == false){
            if(questTracker.haveStartedRace == true){
                countDown.enabled = true;
                currentTime -= 1 * Time.deltaTime;
                Debug.Log(currentTime);
                if(currentTime < 0.5){
                countDown.text = currentTime.ToString("GO");
                }else{
                countDown.text = currentTime.ToString("0");
                }

                if(currentTime <= -1){
                    currentTime = 3;
                    countDown.enabled = false;

                    //npc.firstkeyPressed = false;
                    //npc.startQuest = false;
                }
             }else{
                countDown.enabled = false;
             }
        }

        if(questComplete == true){
            questComplete = true;
            countDown.enabled = false;

        }
    }
}
