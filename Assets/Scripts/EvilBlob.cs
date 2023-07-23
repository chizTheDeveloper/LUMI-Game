using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvilBlob : MonoBehaviour
{

    public float hurtRadius = 30f;
    public Player player;
    Transform target;

    public float speed;
    public Transform[] moveSpots;
    private int randomSpot;

    private float waitTime;
    public float startWaitTime;

    public GameObject newSP;

    public GameObject gamemanager;
    
    GameObject damageScreen;
    float takingDamage = 0;
    bool wasHit;
    Image damageImg;

    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        waitTime = startWaitTime;

        target = PlayerManager.instance.player.transform;

        randomSpot = Random.Range(0, moveSpots.Length);

        rigidBody = GetComponent<Rigidbody>();

        damageScreen = GameObject.FindWithTag("damage");
        damageScreen.SetActive(true);
        wasHit = false;

        damageImg = damageScreen.GetComponent<Image>();
        damageScreen.GetComponent<Image>().color = new Color(damageImg.color.r, damageImg.color.g, damageImg.color.b, 0.0f);

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if(FindObjectOfType<DialogueManager>().dialogueText.enabled == false){
            if(distance <= hurtRadius)
            {
                player.TakeDamage(1);
                StartCoroutine(WaitForAttack(0.75f));
            }
        }

        /*
        if(wasHit == true){
            damageScreen.GetComponent<Image>().color = new Color(damageImg.color.r, damageImg.color.g, damageImg.color.b, takingDamage);
            takingDamage = takingDamage + 0.3f;
        }
        else if(wasHit == false && takingDamage > -1f){
            damageScreen.GetComponent<Image>().color = new Color(damageImg.color.r, damageImg.color.g, damageImg.color.b, takingDamage);
            takingDamage = takingDamage - 0.7f;
        }
        */

        if(moveSpots.Length > 0){
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

        
    }

    private void OnTriggerEnter(Collider collision)
     {
         if (collision.transform.tag == "bubble")
         {

            Vector3 lastPos = gameObject.transform.position;
            speed = 0f;

            newSP = (GameObject)Instantiate(Resources.Load("updatedSP"));
            newSP.transform.position = lastPos;
            newSP.transform.localScale = new Vector3(400,400,400);
            newSP.GetComponent<XP>().player = player; 
            newSP.GetComponent<XP>().gameManager = gamemanager; 

             // do damage here, for example:
             Destroy(gameObject);
         }
     }

    IEnumerator WaitForAttack(float time)
    {
        yield return new WaitForSeconds(time);
        wasHit = true;
        takingDamage = 0;
        StartCoroutine(DamageRedCooldown(0.75f));
    }

    IEnumerator DamageRedCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        wasHit = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hurtRadius);
    }
}
