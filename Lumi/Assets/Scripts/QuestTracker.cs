using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTracker : MonoBehaviour
{
    public GameObject npc;
    public bool hasStarted;
    public int jellyfishCollected;
    public Vector3 velocity = Vector3.one;
    public GameObject mainCam;
    public GameObject octoCam;
    public Vector3 currPos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartQuests(5));
        hasStarted = false;
        jellyfishCollected = 0;
        mainCam.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(hasStarted == true){
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + ((float)Mathf.Sin(Time.time) * 3), transform.position.z);
            Vector3 newLocation = (transform.forward * 20) + pos;
            Vector3 smooth = Vector3.SmoothDamp(npc.transform.position, newLocation, ref velocity, 0.3f);
            
            npc.transform.position = smooth;
            octoCam.SetActive(true);
            mainCam.SetActive(false);
        }
    }

    IEnumerator StartQuests(float time)
    {
        yield return new WaitForSeconds(time);
        hasStarted = true;
    }
        
}
