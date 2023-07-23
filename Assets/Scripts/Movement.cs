using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Works with cube
public class Movement : MonoBehaviour
{
    public CharacterController characterController;
    QuestTracker questTracker;
    QuestTracker2 questTracker2;
    QuestTracker3 questTracker3;
    public GameObject orb;
    public GameObject secretEntrance;
    public GameObject octoNPC;
    public float playerSpeed;
    float mouseX;
    float mouseY;
    float joystickX;
    float joystickY;
    public bool gameBegins;
    public bool orbFound;
    public int checkCount;
    bool boosting;
    public Material colourChange;
    public float distancePlayerOcto;
    public int hintCount;

    public bool freezeRotation;
    public bool onlyRotate;
    public int pause;

    public GameObject manta;
    public GameObject krill;

    private Animator animator;
    private Animator animator2;

  
    public KeyCode interactKeyMAP;

    public Camera camera; 
    CameraFollow cameraFollow;

    //GameObject[] enemies;
    List<GameObject> enemies;
    int closeToEnemy;
    bool anyEnemies;

    public GameObject gameManager;
    public GameObject player;

    public GameObject leaveSign;
    public RawImage mapBig;

    private bool mapOpen = false;
    
    public int axisX;
    public int axisY;
    private bool inUse = false;

    AudioSource jellyCollectSound;
    AudioSource orbSound;
    public GameObject orbSoundGO;
    public GameObject jellySoundObj;
    public bool newJelly;

    public Image PressE;
    public Image PressA;
    public bool firstkeyPressed;

    GameObject damageScreen;
    float takingDamage = 0;
    Image damageImg;



    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameBegins = false;
        onlyRotate = false;
        orbFound = false;
        checkCount = 0;
        boosting = false;
        hintCount = 0;
        freezeRotation = false;
        pause = 0;
        anyEnemies = false;
        newJelly = false;


        animator = manta.GetComponent<Animator>();
        animator2 = krill.GetComponent<Animator>();
        enemies = new List<GameObject>();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("enemy"));

        cameraFollow = camera.GetComponent<CameraFollow>();

        leaveSign.SetActive(false);
        mapBig.enabled = false;

        jellyCollectSound = jellySoundObj.GetComponent<AudioSource>();
        orbSound = orbSoundGO.GetComponent<AudioSource>();
        
        firstkeyPressed = false;

        damageScreen = GameObject.FindWithTag("damage");
        damageScreen.SetActive(true);
        damageImg = damageScreen.GetComponent<Image>();
        damageScreen.GetComponent<Image>().color = new Color(damageImg.color.r, damageImg.color.g, damageImg.color.b, 0.0f);
    }

    void Update()
    {
        if(newJelly == true){
            jellyCollectSound.Play();
            newJelly = false;
        }

        enemies.Clear();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("enemy"));

        /*if(enemies.Count <= 0 && GetComponent<QuestTrackerBoss>() == null){
            damageScreen.GetComponent<Image>().color = new Color(damageImg.color.r, damageImg.color.g, damageImg.color.b, takingDamage);
            takingDamage = takingDamage - 0.7f;
        }*/

        anyEnemies = false;

        // Search for any enemies within range
        for(int i = 0; i < enemies.Count; i++){
            if(Vector3.Distance(transform.position, enemies[i].transform.position) < 200){
                anyEnemies = true;
                //Debug.Log(Vector3.Distance(transform.position, enemies[i].transform.position));
            }
        }

        // Change value for camera to zoom out when enemies are nearby
        if(anyEnemies == true){
            cameraFollow.changeLookAt = true;
            //anyEnemies = false;
        }
        else{
            cameraFollow.changeLookAt = false;
            cameraFollow.pushCam = true;
            Debug.Log("NO ENEMIES");
        }

        if(gameBegins == true && freezeRotation == false){
            Move();
        }
        else if(freezeRotation == true){
            mouseX += Input.GetAxis("Mouse X");
            mouseY += Input.GetAxis("Mouse Y");

            mouseX += Input.GetAxis("Joystick X") * 100f;
            mouseY += Input.GetAxis("Joystick Y") * 100f;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-mouseY * 0.6f, mouseX * 0.6f, 0), 2.5f * Time.deltaTime);
        }
        else if(pause == 1){
            
        }
        else if(pause == 2){
            transform.rotation = Quaternion.Euler(0, 0, 0);
            StartCoroutine(PauseRotation(1f));
        }
        else{
            //float moveX = Input.GetAxis("Horizontal");
            //float moveY = Input.GetAxis("Vertical");

            // Getting mouse positions
            //mouseX += Input.GetAxis("Mouse X");
           //mouseY += Input.GetAxis("Mouse Y");

            // Vector 3 to move player
            //Vector3 move = transform.forward * moveY + transform.right * moveX;
            
            // Rotation using mouse position (to look around), x2 for faster rotation)
            //transform.localRotation = Quaternion.Euler(-mouseY * 0.6f, mouseX * 0.6f, 0);

            //Vector3 bobbing = new Vector3(transform.position.x, transform.position.y + ((float)Mathf.Sin(Time.time) * 0.1f), transform.position.z);
            //transform.position = bobbing;
        }

    
    
    }

    public void Move()
    {
        // For WASD movement
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Getting mouse positions
        if(!gameManager.GetComponent<PauseMenu>().activity){
        mouseX += Input.GetAxis("Mouse X") * 1f;
        mouseY += Input.GetAxis("Mouse Y") * 1f;

        mouseX += Input.GetAxis("Joystick X") * 100f;
        mouseY += Input.GetAxis("Joystick Y") * 100f;

         //map
         if(Input.GetKeyDown("m") || Input.GetKeyDown("joystick button 2")){
            if(mapOpen == false){
            mapBig.enabled = true;
            Debug.Log("OPEN MAP");
            mapOpen = true;
            }else{
            mapBig.enabled = false;
            mapOpen = false;
            }
        }

        }

        // Vector 3 to move player
        Vector3 move = transform.forward * moveY + transform.right * moveX;

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetAxisRaw("Boost") != 0){

            if(inUse == false){
            boosting = true;
            StartCoroutine(BoostDuration(0.7f));
            inUse = true;
            animator.speed = 3f;
            }
            
        }

        
        if(boosting == true){
            if(playerSpeed < 90){
                // Quickly increase speed when boost has be used
                playerSpeed = playerSpeed + 2;
            }
        }
        else{
            if(playerSpeed > 60){
                // Slowly reduce speed when boost isn't in use
                playerSpeed--;
            }
        
        }

        // Rotation using mouse position (to look around)
        transform.localRotation = Quaternion.Euler(-mouseY * 0.6f, mouseX * 0.6f, 0);
        
        //only move if not in text
        if(FindObjectOfType<DialogueManager>().dialogueText.enabled == false){

        // Move player
        characterController.Move(playerSpeed * Time.deltaTime * move);

         }

        if(move != Vector3.zero && FindObjectOfType<DialogueManager>().dialogueText.enabled == false){
            animator.SetBool("isSwimming", true);
            animator2.SetBool("isSwimming", true);
        }else{
            animator.SetBool("isSwimming", false);
            animator2.SetBool("isSwimming", false);
        }

        

        if(Input.GetAxisRaw("Boost") == 0){
            inUse = false;
        }

        // Distance from secret area

        questTracker = GetComponent<QuestTracker>();
        if(questTracker != null){
            float secretAreaDistance = Vector3.Distance(transform.position, secretEntrance.transform.position);

            if(secretAreaDistance < 65 && checkCount == 0){
                questTracker = GetComponent<QuestTracker>();
                questTracker.progress.Add("nearSecretArea");
                Debug.Log("SECRET AREA NEARBY");
                questTracker.interactingWith = "companion";
                checkCount++;
            }
        }

        //Debug.Log(secretAreaDistance);
        
        distancePlayerOcto = Vector3.Distance(transform.position, octoNPC.transform.position);

        //questTracker = GetComponent<QuestTracker>();
       //if(distancePlayerOcto < 20 && hintCount == 0 && questTracker){

        if(distancePlayerOcto < 20){

            /*if(gameManager.GetComponent<KeyorCont>().keyboard == true){
               PressE.enabled = true;
            }
            else{
               PressA.enabled = true;
            }*/

            if((Input.GetKeyDown("e") || Input.GetKeyDown("joystick button 0")) && firstkeyPressed == false && orbFound == false){
                QuestTracker questTracker = GetComponent<QuestTracker>();
                QuestTracker2 questTracker2 = GetComponent<QuestTracker2>();
                QuestTracker3 questTracker3 = GetComponent<QuestTracker3>();

                if(questTracker != null){
                    questTracker.progress.Add("octoTip");
                    questTracker.interactingWith = "npc";
                }
                else if(questTracker2 != null){
                    questTracker2.progress.Add("octoTipL2");
                    questTracker2.interactingWith = "octoL2";
                }
                else if(questTracker3 != null){
                    questTracker3.progress.Add("octoTipL3");
                    questTracker3.interactingWith = "octoL3";
                }

                //hintCount = 1;
                firstkeyPressed = true;
            }else if((Input.GetKeyDown("e") || Input.GetKeyDown("joystick button 0")) && firstkeyPressed == true){
                firstkeyPressed = false;
            }

            if(FindObjectOfType<DialogueManager>().dialogueText.enabled == false && orbFound == false){   
                if(gameManager.GetComponent<KeyorCont>().keyboard == true){
                    PressE.enabled = true;
                }else{
                    PressA.enabled = true;
                }
            }
            else{
                PressE.enabled = false;
                PressA.enabled = false;
            }
        }

/*
        if(distancePlayerOcto < 20 && questTracker.jellyCaught == 3){
            questTracker.jellyCaught = 4;
            questTracker.progress.Add("congratsJelly");
            questTracker.interactingWith = "npc";
        }
        */
    }

    private void OnTriggerEnter(Collider collision){
        if(collision.transform.tag == "jelly"){
            Debug.Log("Jelly");
            collision.GetComponent<Renderer>().material = colourChange;
        }

         if(collision.transform.tag == "orb"){
            if(GetComponent<QuestTracker3>() != null)
            {
                questTracker3 = GetComponent<QuestTracker3>();
                questTracker3.progress.Add("allOrbsFound");
                questTracker3.interactingWith = "krillL3end";
            }
            else if(GetComponent<QuestTracker2>() != null)
            {
                questTracker2 = GetComponent<QuestTracker2>();
                questTracker2.progress.Add("secondOrbFound");
                questTracker2.interactingWith = "octoL2";
            }

            orbFound = true;
            Destroy(GameObject.FindWithTag("orb"));
            Destroy(GameObject.FindWithTag("octoChat"));
            Destroy(GameObject.FindWithTag("octoChatIcon"));
            leaveSign.SetActive(true);
            Destroy(GameObject.FindWithTag("walldestroy"));

            questTracker = GetComponent<QuestTracker>();
            questTracker.progress.Add("foundOrb");

            orbSound.Play();
        }

        // Couldn't get this to work
        /*
        if(collision.transform.tag == "wallDestroy"){
            if(GetComponent<QuestTracker3>() != null)
            {
                questTracker3 = GetComponent<QuestTracker3>();
                questTracker3.progress.Add("triedToExit");
                questTracker3.interactingWith = "krill";
            }
            else if(GetComponent<QuestTracker2>() != null)
            {
                questTracker2 = GetComponent<QuestTracker2>();
                questTracker2.progress.Add("triedToExit");
                questTracker2.interactingWith = "krill";
            }
            else if(GetComponent<QuestTracker>() != null){
                questTracker = GetComponent<QuestTracker>();
                questTracker.progress.Add("triedToExit");
                questTracker.interactingWith = "krill";
            }

            Debug.Log("Wall");
        }
        */


        if(collision.transform.tag == "nextLevel"){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

/*
        if(collision.transform.tag == "nextLevel2"){
            SceneManager.LoadScene("Scene3");
        }

        if(collision.transform.tag == "nextLevel3"){
            Debug.Log("next scene");
            SceneManager.LoadScene("BossScene");
        }
        */
    }

    IEnumerator BoostDuration(float time)
    {
        yield return new WaitForSeconds(time);
        boosting = false;
        animator.speed = 1;
    }

    IEnumerator PauseRotation(float time){
        yield return new WaitForSeconds(time);
        pause = 1;
        gameBegins = true;
    }

}
