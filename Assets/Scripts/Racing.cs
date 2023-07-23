using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class Racing : MonoBehaviour
{
    public GameObject opponent;

    QuestTracker3 questTracker;

    public CountDownTimer2 timer;

    Vector3[] checkpoints;
    Vector3 velocity = Vector3.one;

    public float enemySpeed;
    float enemyCooldown;
    int step;
    float timeStamp;
    int winningZ = 6000;
    bool playerWon;
    bool oppBoosting;
    public Player player;

    float riseY;
    GameObject raceTrack;
    Movement movement;
    float mouseX;
    float mouseY;

    int raceStarted;

    public Image anglerQuest;

    public Animator animator;

    public GameObject coolDown;

    private PowerUp PowerUp;

    void Start()
    {
        checkpoints = new Vector3[4];
        checkpoints[0] = new Vector3(4593.3f, 57f, 5263.8f);
        checkpoints[1] = new Vector3(4555f, 67.4f, 5645f);
        checkpoints[2] = new Vector3(4553f, 67.4f, 5854f);
        checkpoints[3] = new Vector3(4551f, 67.4f, 6018.1f);

        enemySpeed = 58;
        step = 0;
        questTracker = GetComponent<QuestTracker3>();
        playerWon = false;
        raceStarted = 0;

        movement = GetComponent<Movement>();

        animator = opponent.GetComponent<Animator>();

        oppBoosting = false;

        PowerUp = player.GetComponent<PowerUp>();
    }

    void Update()
    {
        if(questTracker.progress.LastOrDefault() == "startRace"){
            if(raceStarted == 0){
                movement.freezeRotation = true;
                movement.gameBegins = false;
                StartCoroutine(RaceCountdown(2.8f));
            }
            else if(raceStarted == 1){
                Race();
            }
        }

        if(raceStarted == 0){
            animator.SetBool("isRacing", false);
        }else if(raceStarted == 1){
            animator.SetBool("isRacing", true);
        }
    }

    void Race()
    {
// Check if the opponent is in range of race checkpoints
            for(int i = 0; i < 4; i++){
                float checkpointDistance = Vector3.Distance(opponent.transform.position, checkpoints[i]);

                // If very close to checkpoint, change step so we can move towards the next checkpoint
                if(checkpointDistance < 10 && timeStamp <= Time.time){
                    timeStamp = Time.time + 0.5f;
                    step = step + 1;
                }
            }

            // Distance between the opponent and the player
            float distanceFromPlayer = Vector3.Distance(opponent.transform.position, transform.position);

            if(Input.GetKeyDown(KeyCode.Space) || Input.GetAxisRaw("Boost") != 0){
                oppBoosting = true;
                StartCoroutine(OppBoostDuration(0.4f));
            }
            else{
                if(oppBoosting == true){
                    if(enemySpeed < 93 && enemyCooldown <= Time.time){
                        enemyCooldown = Time.time + 0.05f;
                        enemySpeed = enemySpeed + 4;
                    }
                }
                else{
                    if(enemySpeed > 58){
                        enemySpeed--;
                    }
                }
            }

            // Move the opponent towards the checkpoint
            float moveit =  enemySpeed * Time.deltaTime;

            if(step < 4){
                opponent.transform.position = Vector3.MoveTowards(opponent.transform.position, checkpoints[step], moveit);
            }

            if(opponent.transform.position.z > winningZ && transform.position.z < winningZ){
                playerWon = false;
                questTracker.haveStartedRace = false;
                questTracker.progress.Add("lostRace");
                questTracker.interactingWith = "fish";
                step = 0;
                timer.questComplete = false;
                raceStarted = 0;
            }
            else if(opponent.transform.position.z < winningZ && transform.position.z > winningZ && playerWon != false){
                playerWon = true;
                questTracker.progress.Add("wonRace");
                questTracker.interactingWith = "fish";
                player.SQ3 = true;
                anglerQuest.enabled = false;
                player.playerLevel++;
                timer.questComplete = true;
                raceStarted = 0;
                Destroy(GameObject.FindWithTag("questDest"));
                for(int i = 0; i< PowerUp.slots.Length; i++){
                    if(PowerUp.isFull[i] == false){

                        PowerUp.isFull[i] = true;
                        Instantiate(coolDown, PowerUp.slots[i].transform, false);
                        break;
                    }
                }
            }
        
        else{
            playerWon = true;
        }
    }
    

    IEnumerator OppBoostDuration(float time)
    {
        yield return new WaitForSeconds(time);
        oppBoosting = false;
    }

    IEnumerator RaceCountdown(float time)
    {
        yield return new WaitForSeconds(time);
        raceStarted = 1;
        questTracker.haveStartedRace = false;
        movement.gameBegins = true;
        movement.pause = 0;
        movement.freezeRotation = false;
    }
}
