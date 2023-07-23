using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class EnemyAI : MonoBehaviour
{

    public float lookRadius = 5f;
    public float attackRadius = 0f;
    public float stopRadius = 0f;
    public float warning = 160f;

    QuestTracker questTracker;
    public GameObject playerGO;

    Transform target;
    NavMeshAgent agent;

    public Player player;
    public float FollowSpeed;

    public Transform[] points;
    int current;
    public float speed;

    public int damage;

    private Animator animator;

    public GameObject gameManager;

    public GameObject[] kords;

    Rigidbody rigidBody;

    private float attackCooldown = 0f;
    public float attackSpeed = 1f;

    GameObject damageScreen;
    float takingDamage = 0;
    bool wasHit;
    Image damageImg;

    public bool getAttacked = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();

        damageScreen = GameObject.FindWithTag("damage");
        damageScreen.SetActive(true);
        wasHit = false;

        damageImg = damageScreen.GetComponent<Image>();
        damageScreen.GetComponent<Image>().color = new Color(damageImg.color.r, damageImg.color.g, damageImg.color.b, 0.0f);

        current = 0;

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

        attackCooldown -= Time.deltaTime;

        if(wasHit == true){
            damageScreen.GetComponent<Image>().color = new Color(damageImg.color.r, damageImg.color.g, damageImg.color.b, takingDamage);
            takingDamage = takingDamage + 0.3f;
        }
        else if(wasHit == false && takingDamage > -2f){
            damageScreen.GetComponent<Image>().color = new Color(damageImg.color.r, damageImg.color.g, damageImg.color.b, takingDamage);
            takingDamage = takingDamage - 0.7f;
        }

        if(!gameManager.GetComponent<PauseMenu>().activity && FindObjectOfType<DialogueManager>().dialogueText.enabled == false){
        if(distance <= lookRadius)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position,FollowSpeed);
            FollowSpeed = 0.8f;
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
            //player.TakeDamage(1);
            Damage(1);

            if(getAttacked == true){
                animator.Play("Attack", 0,0);
            }
        }

        if(distance <= warning){
           // questTracker = playerGO.GetComponent<QuestTracker>();
            //if(!questTracker.progress.Contains("howToHunt")){
            //    questTracker.progress.Add("howToHunt");
            //    questTracker.interactingWith = "companion";
            //}
        }


         if(distance <= stopRadius){
            FollowSpeed = 0;
        }
        }

         if(FindObjectOfType<DialogueManager>().dialogueText.enabled == true){
            animator.speed = 0;
        }else if(FindObjectOfType<DialogueManager>().dialogueText.enabled == false){
            animator.speed = 1;
        }

       if(gameObject.GetComponent<EnemyHealth>().currentHealth <= 0){
        Debug.Log("dead");
         animator.Play("Attacked", 0,0);
         damageScreen.GetComponent<Image>().color = new Color(damageImg.color.r, damageImg.color.g, damageImg.color.b, 0);
         Destroy(gameObject);
        }

          if(player.currentXP == 5){
            damage = 8;
         }else if(player.SQ1 == false){
            damage = 1;
         }else if(player.SQ1 == true){
            damage = 2;
         }
        
         
         //else if(player.playerLevel == 2){
         //   damage = 3;
        // }else if(player.playerLevel == 3){
        //    damage = 4;
       //  }
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

    public void Damage(int damage)
    {
        if (attackCooldown <= 0f)
        {
            //player.currentHealth -= damage;
            //player.healthBar.SetHealth(player.currentHealth);
            attackCooldown = 5f/attackSpeed;
            getAttacked = true;

            StartCoroutine(WaitForAttack(0.75f));
        }else{
            getAttacked = false;
        }
        
    }


    private void OnTriggerEnter(Collider collision)
     {
         if (collision.transform.tag == "bubble")
         {
             // do damage here, for example:
             Debug.Log(collision.transform.name);
             gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
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

    IEnumerator DamageRedCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        wasHit = false;
    }

    IEnumerator WaitForAttack(float time)
    {
        yield return new WaitForSeconds(time);
        wasHit = true;
        takingDamage = 0;
        getAttacked = true;
        player.currentHealth -= 1;
        player.healthBar.SetHealth(player.currentHealth);
        StartCoroutine(DamageRedCooldown(0.75f));
    }
}
