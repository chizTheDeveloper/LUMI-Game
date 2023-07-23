using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ExtraSpecialEnemy : MonoBehaviour
{

    public float lookRadius = 5f;
    public float attackRadius = 0f;
    public float stopRadius = 0f;
    public float warning = 160f;

    public int damage;

    QuestTracker questTracker;
    public GameObject playerGO;
    public GameObject orb;

    Transform target;
    NavMeshAgent agent;

    public Player player;
    public float FollowSpeed;

    public Transform[] points;
    int current;
    public float speed;

    public bool keyAppear;

    private Animator animator;

    public GameObject gameManager;

    public GameObject[] kords;

    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();

        current = 0;

        keyAppear = false;

        rigidBody = GetComponent<Rigidbody>();

        kords = GameObject.FindGameObjectsWithTag("enemy");

        for(int i = 0; i < kords.Length; i++){
            Physics.IgnoreCollision(GetComponent<Collider>(), kords[i].GetComponent<Collider>(), true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if(!gameManager.GetComponent<PauseMenu>().activity && FindObjectOfType<DialogueManager>().dialogueText.enabled == false){
        
        if(distance <= stopRadius){
            FollowSpeed = 0;
        }

        if(distance <= lookRadius)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position,FollowSpeed);
            FollowSpeed = 0.4f;
            transform.LookAt(player.transform);
        
        }else{
            FollowSpeed = 0;

            if(transform.position != points[current].position)
            {
                transform.position = Vector3.MoveTowards(transform.position, points[current].position, speed * Time.deltaTime);
                transform.LookAt(points[current].position);
            }
            else
            {
                current = (current + 1) % points.Length;
            }
        }

        if (distance <= attackRadius)
        {
            player.TakeDamage(1);

            if(player.getAttacked == true){
                animator.Play("Attack", 0,0);
            }
        }


       if(gameObject.GetComponent<SpecialHealth>().currentHealth <= 0){
         Vector3 lastPos = gameObject.transform.position;

         Destroy(gameObject);

         orb = (GameObject)Instantiate(Resources.Load("Electrick orb"));
         orb.transform.position = lastPos;
         //key.transform.localScale = new Vector3(45, 45, 45);
         //key.GetComponent<FinalKey>().gameManager = gameManager;
         
        }

         if(player.currentXP == 5){
            damage = 8;
         }else if(player.SQ1 == false){
            damage = 1;
         }else if(player.SQ1 == true){
            damage = 2;
         }

        }
        
        if(FindObjectOfType<DialogueManager>().dialogueText.enabled == true){
            animator.speed = 0;
        }else if(FindObjectOfType<DialogueManager>().dialogueText.enabled == false){
            animator.speed = 1;
        }
        

        
    }

    /*
   void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,direction.y,direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    */

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, warning);
    }


    public void OnTriggerEnter(Collider collision)
     {
         if (collision.transform.tag == "bubble")
         {
             // do damage here, for example:
             Debug.Log(collision.transform.name);
             gameObject.GetComponent<SpecialHealth>().TakeDamage(damage);
             animator.Play("Attacked", 0,0);
         }
     }

     private void OnCollisionEnter(Collision collision){
            if(collision.gameObject.tag == "enemy"){
                rigidBody.isKinematic = true;
                rigidBody.isKinematic = false;
            }
            else{
                rigidBody.isKinematic = true;
            }
     }
}
