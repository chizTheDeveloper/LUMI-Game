using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossScript : MonoBehaviour
{
    public Transform player;
    public Player playerP;
    public GameObject playerObj;
    //public CharacterController playerCar;

    public GameObject bub;
    public CharacterController enemyPos;
    public Vector3 wayFacing;
    public Vector3 wayFacingBlob;
    public List<GameObject> bubbles;

    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public GameObject gameManager;

    public QuestTrackerBoss questTrackerBoss;

   // public GameObject playerPos;
   // public float TargetDistance;
   // public float FollowSpeed;
   public RaycastHit Shot;

    void Start()
    {
        bubbles = new List<GameObject>();

        questTrackerBoss = playerObj.GetComponent<QuestTrackerBoss>();
    }

    void Update()
    {
        //transform.LookAt(player);
        if(!FindObjectOfType<PauseMenu>().activity && FindObjectOfType<DialogueManager>().dialogueText.enabled == false && questTrackerBoss.cutscene == false){
        AttackPlayer();
        }
    }

    void AttackPlayer()
    {

        if(!alreadyAttacked)
        {
            wayFacing = enemyPos.transform.forward;
            //playerPos = playerCar.transform.position;

            bub = (GameObject)Instantiate(Resources.Load("BLOBKRAKEN"));
            bub.transform.position = enemyPos.transform.position;
            bub.transform.localScale = new Vector3(8, 8, 8);
            bub.GetComponent<BossBlobs>().player = player;
            bub.GetComponent<BossBlobs>().playerObj = playerObj;
            bub.GetComponent<EvilBlob>().player = playerP;
            bub.GetComponent<EvilBlob>().gamemanager = gameManager;

           // bub.transform.LookAt(player);
            bub.transform.Translate(wayFacing * Time.deltaTime * 70);

            bubbles.Add(bub);

            bubbles.RemoveAll(item => item == null);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

        }


        //bub.transform.Translate(wayFacing * Time.deltaTime * 70);

         //for(int i = 0; i < bubbles.Count; i++){

            //bubbles[i].transform.LookAt(player);
            //if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),out Shot)){
            //bubbles[i].transform.position = Vector3.MoveTowards(bub.transform.position, player.position, 1f);
            //}

                //bubbles[i].transform.LookAt(player);
                //float distance = Vector3.Distance (bubbles[i].transform.position, enemyPos.transform.position);

             // }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
