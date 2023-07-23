using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Camera mainCam;
    public GameObject player;
    Vector3 wayFacing;
    public Image crosshair;
    float shootDistance;
    public Player playerScript;

    void Start()
    {
        // Disable the crosshair, set default shoot distance to the distance the bubble is able to travel without special abilities
        crosshair.enabled = false;
        shootDistance = 153f;
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        wayFacing = player.transform.forward;
        wayFacing = new Vector3(wayFacing.x, wayFacing.y - 0.05f, wayFacing.z);

        // Check for special abilities to change the raycast distance for bubble shoot
        if(playerScript.SQ2 == true){
            shootDistance = 272f;
        }else{
            shootDistance = 153f;
        }

        // If the raycast hits something without shooting distance that has the tag "enemy"
        if(Physics.Raycast(player.transform.position, wayFacing, out hit, shootDistance) && hit.transform.tag == "enemy"){
            // Show crosshair, smoothen position of crosshair (which is translated from the raycast hit point to a screen point for the UI image)
            crosshair.enabled = true;
            crosshair.transform.position = Vector3.Lerp(crosshair.transform.position, mainCam.WorldToScreenPoint(hit.point), 1.0f - Mathf.Pow(0.5f, Time.deltaTime * 100f));
        }
        // Raycast for blobs
        else if(Physics.Raycast(player.transform.position, wayFacing, out hit, shootDistance) && hit.transform.tag == "Blob"){
            crosshair.enabled = true;
            crosshair.transform.position = Vector3.Lerp(crosshair.transform.position, mainCam.WorldToScreenPoint(hit.point), 1.0f - Mathf.Pow(0.5f, Time.deltaTime * 100f));
        }
        // Raycast for boss blobs
        else if(Physics.Raycast(player.transform.position, wayFacing, out hit, shootDistance) && hit.transform.tag == "bossblob"){
            crosshair.enabled = true;
            crosshair.transform.position = Vector3.Lerp(crosshair.transform.position, mainCam.WorldToScreenPoint(hit.point), 1.0f - Mathf.Pow(0.5f, Time.deltaTime * 100f));
        }
        // Raycast for kraken
        else if(Physics.Raycast(player.transform.position, wayFacing, out hit, shootDistance) && hit.transform.tag == "Kraken"){
            crosshair.enabled = true;
            crosshair.transform.position = Vector3.Lerp(crosshair.transform.position, mainCam.WorldToScreenPoint(hit.point), 1.0f - Mathf.Pow(0.5f, Time.deltaTime * 100f));
        }
        else{
            crosshair.enabled = false;
            //Debug.Log(hit.transform.tag);
        }
    }
}
