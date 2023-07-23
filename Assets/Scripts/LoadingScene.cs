using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public GameObject LoadingScreen;
    public GameObject LoadingScreen2;
    public GameObject mainMenu;
    public Slider LoadingBarFill;
    public bool isActive = true;




    public void LoadScene()
    {
        if(isActive == true){
            LoadingScreen.SetActive(true);
            mainMenu.SetActive(false);
            isActive = false;
        }else if(isActive == false){
            LoadingScreen.SetActive(false);
            mainMenu.SetActive(true);
            isActive = true;
        }
    }
    

     public void LoadScene2()
    {
        if(isActive == true){
            LoadingScreen2.SetActive(true);
            mainMenu.SetActive(false);
            isActive = false;
        }else if(isActive == false){
            LoadingScreen2.SetActive(false);
            mainMenu.SetActive(true);
            isActive = true;
        }
    }

   
    
    


}