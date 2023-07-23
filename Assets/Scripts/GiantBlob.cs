using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBlob : MonoBehaviour
{


    public int counter;
    public float hurtRadius = 60f;
    public Player player;
    Transform target;

    void Start()
    {
        counter = 0;
        target = PlayerManager.instance.player.transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

         if(distance <= hurtRadius)
        {
            player.TakeDamage(1);
        }
    }

    private void OnTriggerEnter(Collider collision)
     {

         if (collision.transform.tag == "bubble")
         {
            counter++;

            if(counter == 4){
                gameObject.SetActive(false);
                Destroy(GameObject.FindWithTag("questDest"));
            }

            if(player.currentXP == 5){
                gameObject.SetActive(false);
                Destroy(GameObject.FindWithTag("questDest"));
            }
         }
     }

        void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hurtRadius);
    }
}
