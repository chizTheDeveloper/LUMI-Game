using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{

    public int maxHealth = 20;
    public int currentHealth;

    public int maxXP = 5;
    public int currentXP;

    public XPBar xpBar;

    public HealthBar healthBar;

    QuestTracker questTracker;

    [SerializeField] public TMP_Text custumText;
    [SerializeField] public TMP_Text custumText2;
    [SerializeField] public TMP_Text custumText3;
    [SerializeField] public TMP_Text custumText4;
    [SerializeField] public TMP_Text custumText5;
    [SerializeField] public TMP_Text custumText6;

    public float attackSpeed = 1f;
    private float attackCooldown = 0f;

    public int numOfHearts;

    public int playerLevel;

    public bool SQ1;
    public bool SQ2;
    public bool SQ3;

    public bool getAttacked = false;

    GameObject damageScreen;
    float takingDamage = 0;
    bool wasHit;
    Image damageImg;

    public QuestTrackerBoss questTrackerBoss;

    public GameObject damageUp;
    public GameObject fartherBlub;
    public GameObject coolDown;

    private PowerUp PowerUp;

    GameObject deathScreen;
    Image deathImg;
    float dying;
    bool triggerDeath;

    // Start is called before the first frame update
    void Start()
    {

        PowerUp = gameObject.GetComponent<PowerUp>();

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        damageScreen = GameObject.FindWithTag("damage");
        damageScreen.SetActive(true);
        wasHit = false;

        damageImg = damageScreen.GetComponent<Image>();
        damageScreen.GetComponent<Image>().color = new Color(damageImg.color.r, damageImg.color.g, damageImg.color.b, 0.0f);

        deathScreen = GameObject.FindWithTag("deathScreen");
        damageScreen.SetActive(true);

        deathImg = damageScreen.GetComponent<Image>();
        deathScreen.GetComponent<Image>().color = new Color(deathImg.color.r, deathImg.color.g, deathImg.color.b, 0.0f);
        dying = 0;
        triggerDeath = false;

        if(currentHealth <= 0){
            currentHealth = maxHealth;
            currentXP = 0;
        }

        if (sceneName == "SampleScene") 
         {
            currentXP = 0;
            xpBar.SetMaxXP(maxXP);
            xpBar.SetXP(currentXP);
            numOfHearts = 0;
            playerLevel = 0;
            currentHealth = maxHealth;
            healthBar.SetHealth(maxHealth);
            SQ1 = false;
            SQ2 = false;
            SQ3 = false;
         }

         if (sceneName == "Scene2") 
         {  
            xpBar.SetXP(currentXP);
            SQ2 = false;
            playerLevel = 0;
            healthBar.SetHealth(currentHealth);
            if(SQ1 == true){
                playerLevel++;
                for(int i = 0; i< PowerUp.slots.Length; i++){
                    if(PowerUp.isFull[i] == false){

                        PowerUp.isFull[i] = true;
                        Instantiate(damageUp, PowerUp.slots[i].transform, false);
                        break;
                    }
                }
            }
         }

        if (sceneName == "Scene3") 
         {
            xpBar.SetXP(currentXP);
            SQ3 = false;
            playerLevel = 0;
            healthBar.SetHealth(currentHealth);
            if(SQ1 == true){
                playerLevel++;
                for(int i = 0; i< PowerUp.slots.Length; i++){
                    if(PowerUp.isFull[i] == false){

                        PowerUp.isFull[i] = true;
                        Instantiate(damageUp, PowerUp.slots[i].transform, false);
                        break;
                    }
                }
            }
            if(SQ2 == true){
                playerLevel++;
                for(int i = 0; i< PowerUp.slots.Length; i++){
                    if(PowerUp.isFull[i] == false){

                        PowerUp.isFull[i] = true;
                        Instantiate(fartherBlub, PowerUp.slots[i].transform, false);
                        break;
                    }
                }
            }
         }

        if (sceneName == "BossScene") 
         {
            xpBar.SetXP(currentXP);
            playerLevel = 0;
            healthBar.SetHealth(currentHealth);
            if(SQ1 == true){
                playerLevel++;
                for(int i = 0; i< PowerUp.slots.Length; i++){
                    if(PowerUp.isFull[i] == false){

                        PowerUp.isFull[i] = true;
                        Instantiate(damageUp, PowerUp.slots[i].transform, false);
                        break;
                    }
                }
            }
            if(SQ2 == true){
                playerLevel++;
                for(int i = 0; i< PowerUp.slots.Length; i++){
                    if(PowerUp.isFull[i] == false){

                        PowerUp.isFull[i] = true;
                        Instantiate(fartherBlub, PowerUp.slots[i].transform, false);
                        break;
                    }
                }
            }
            if(SQ3 == true){
                playerLevel++;
                for(int i = 0; i< PowerUp.slots.Length; i++){
                    if(PowerUp.isFull[i] == false){

                        PowerUp.isFull[i] = true;
                        Instantiate(coolDown, PowerUp.slots[i].transform, false);
                        break;
                    }
                }
            }
         }


        

            if(numOfHearts == 6){
                custumText6.enabled = true;
            } else if(numOfHearts == 5){
                custumText5.enabled = true;
            }else if(numOfHearts == 4){
                custumText4.enabled = true;
            }else if(numOfHearts == 3){
                custumText3.enabled = true;
            }else if(numOfHearts == 2){
                custumText2.enabled = true;
            }else if(numOfHearts == 1){
                custumText.enabled = true;
            }
         
            questTrackerBoss = gameObject.GetComponent<QuestTrackerBoss>();


    }

    // Update is called once per frame
    void Update()
    {
        attackCooldown -= Time.deltaTime;
        Debug.Log(currentHealth);

        if(wasHit == true){
            damageScreen.GetComponent<Image>().color = new Color(damageImg.color.r, damageImg.color.g, damageImg.color.b, takingDamage);
            takingDamage = takingDamage + 0.3f;
        }
        else if(wasHit == false && takingDamage > -1f){
            damageScreen.GetComponent<Image>().color = new Color(damageImg.color.r, damageImg.color.g, damageImg.color.b, takingDamage);
            takingDamage = takingDamage - 0.3f;
        }
        
        if(currentHealth <= 0 && numOfHearts == 6){
            custumText6.enabled = false;
            custumText5.enabled = true;
            healthBar.SetHealth(maxHealth);
            currentHealth = maxHealth;
            numOfHearts--;
        }else if(currentHealth <= 0 && numOfHearts == 5){
            custumText5.enabled = false;
            custumText4.enabled = true;
            healthBar.SetHealth(maxHealth);
            currentHealth = maxHealth;
            numOfHearts--;
        }else if(currentHealth <= 0 && numOfHearts == 4){
            custumText4.enabled = false;
            custumText3.enabled = true;
            healthBar.SetHealth(maxHealth);
            currentHealth = maxHealth;
            numOfHearts--;
        }else if(currentHealth <= 0 && numOfHearts == 3){
            custumText3.enabled = false;
            custumText2.enabled = true;
            healthBar.SetHealth(maxHealth);
            currentHealth = maxHealth;
            numOfHearts--;
        }else if(currentHealth <= 0 && numOfHearts == 2){
            custumText2.enabled = false;
            custumText.enabled = true;
            healthBar.SetHealth(maxHealth);
            currentHealth = maxHealth;
            numOfHearts--;
        }else if(currentHealth <= 0 && numOfHearts == 1){
            custumText.enabled = false;
            healthBar.SetHealth(maxHealth);
            currentHealth = maxHealth;
            numOfHearts--;
        }else if (currentHealth <= 0 && numOfHearts <= 0 && triggerDeath == false) {
            StartCoroutine(DeathFade(3.5f));
            triggerDeath = true;
        }

        if(triggerDeath == true){
            deathScreen.GetComponent<Image>().color = new Color(deathImg.color.r, deathImg.color.g, deathImg.color.b, dying);
            dying = dying + 0.07f;
        }
    }

    public void TakeDamage(int damage)
    {
        if (attackCooldown <= 0f)
        {
            attackCooldown = 3f/attackSpeed;
            getAttacked = true;
            StartCoroutine(WaitForAttack(0));
        }else{
            getAttacked = false;
        }
    }

    public void GainXP(int gain)
    {
        currentXP += gain;
        xpBar.SetXP(currentXP);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "key")
        {
             Debug.Log("KEY");
             Destroy(GameObject.Find("secretCaveDoor"));
             Destroy(GameObject.FindWithTag("key"));
             questTracker = GetComponent<QuestTracker>();
             questTracker.keyFound = true;
             questTracker.progress.Add("pickedUpKey");
            questTracker.interactingWith = "companion";
        }
        

        if(other.transform.tag == "heart")
        {
             numOfHearts++;
        }

    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bossblob"){
            //currentHealth -= 1;
            //healthBar.SetHealth(currentHealth);
            StartCoroutine(WaitForAttack(0));
        }

        if (collision.gameObject.tag == "KrakenLeg" && questTrackerBoss.cutscene == false){
            //currentHealth -= 1;
            healthBar.SetHealth(currentHealth);
            StartCoroutine(WaitForAttack(0));
        }
    }
    


    void OnDisable()
{
    PlayerPrefs.SetInt("hearts", numOfHearts);
    PlayerPrefs.SetInt("level", playerLevel);
    PlayerPrefs.SetInt("health", currentHealth);
    PlayerPrefs.SetInt("special", currentXP);
    PlayerPrefs.SetInt("sidequest1", (SQ1 ? 1 : 0));
    PlayerPrefs.SetInt("sidequest2", (SQ2 ? 1 : 0));
    PlayerPrefs.SetInt("sidequest3", (SQ3 ? 1 : 0));
}


void OnEnable()
{
    numOfHearts  =  PlayerPrefs.GetInt("hearts");
    playerLevel = PlayerPrefs.GetInt("level");
    currentHealth = PlayerPrefs.GetInt("health");
    currentXP = PlayerPrefs.GetInt("special");
    SQ1 = (PlayerPrefs.GetInt("sidequest1") != 0);
    SQ2 = (PlayerPrefs.GetInt("sidequest2") != 0);
    SQ3 = (PlayerPrefs.GetInt("sidequest3") != 0);

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
        currentHealth -= 1;
        healthBar.SetHealth(currentHealth);
        StartCoroutine(DamageRedCooldown(0.75f));
    }

    IEnumerator DeathFade(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
