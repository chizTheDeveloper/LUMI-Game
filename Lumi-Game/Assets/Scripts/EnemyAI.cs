using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{

    public float lookRadius = 5f;
    public float attackRadius = 0f;
    public float stopRadius = 0f;

    Transform target;
    NavMeshAgent agent;

    public Player player;
    public float FollowSpeed;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if(distance <= lookRadius)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position,FollowSpeed);
            FollowSpeed = 0.3f;
            transform.LookAt(player.transform);

        
        }else{
            FollowSpeed = 0;
        }

        if (distance <= attackRadius)
        {
            player.TakeDamage(1);
            Debug.Log("damage");
        }

         if(distance <= stopRadius){
            FollowSpeed = 0;
        }

       if(gameObject.GetComponent<EnemyHealth>().currentHealth == 0){
         Destroy(gameObject);
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
    }


    private void OnTriggerEnter(Collider collision)
     {
         if (collision.transform.tag == "bubble")
         {
             // do damage here, for example:
             Debug.Log(collision.transform.name);
             gameObject.GetComponent<EnemyHealth>().TakeDamage(1);
         }
     }
}
