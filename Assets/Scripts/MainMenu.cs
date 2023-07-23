using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public bool keyboard;

    public GameObject LoadingScreen;

    public GameObject gameCont;
        


        public void PlayGameKEY()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            gameCont.GetComponent<KeyorCont>().keyboard = true;

        }


        public void PlayGameCONT()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

             gameCont.GetComponent<KeyorCont>().keyboard = false;

        }



}
