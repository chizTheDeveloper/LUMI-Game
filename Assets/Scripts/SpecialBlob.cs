using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBlob : MonoBehaviour
{

    public CountDownTimer timer;

    float hurtRadius = 70f;

    public Transform[] moveSpots;
    private int randomSpot;

    private float waitTime;
    public float startWaitTime;
    public float speed;

    public Player player;
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        timer = GameObject.FindGameObjectWithTag("timer").GetComponent<CountDownTimer>();

        waitTime = startWaitTime;

        target = PlayerManager.instance.player.transform;

        randomSpot = Random.Range(0, moveSpots.Length);

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

         if(distance <= hurtRadius)
        {
            player.TakeDamage(1);
        }

        transform.position = Vector3.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f){
            if(waitTime < 0){
                randomSpot = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
            }else{
                waitTime -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
     {
         if (collision.transform.tag == "bubble")
         {
            gameObject.SetActive(false);
            timer.collected++;

         }
     }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hurtRadius);
    }
}
