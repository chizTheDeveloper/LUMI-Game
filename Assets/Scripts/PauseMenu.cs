using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject ControlsMenu;
    public bool activity = false;

    public GameObject gameManager;

    public Image keyboard;
    public Image controller;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        ControlsMenu.SetActive(false);
        Time.timeScale = 0f;
        activity = true;
        if (Input.GetKeyDown(KeyCode.Escape)){
            Cursor.visible = true;
            gameManager.GetComponent<GamepadCursor>().previousControlScheme = "Keyboard&Mouse";
        }else if(Input.GetKeyDown("joystick button 7")){
            gameManager.GetComponent<GamepadCursor>().cursorTransform.gameObject.SetActive(true);
            gameManager.GetComponent<GamepadCursor>().previousControlScheme = "Gamepad";
        }
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        Time.timeScale = 1f;
        activity = false;
        Cursor.visible = false;
        gameManager.GetComponent<GamepadCursor>().cursorTransform.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        gameManager.GetComponent<GamepadCursor>().previousControlScheme = "";
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void Controls()
    {
        ControlsMenu.SetActive(true);
        pauseMenu.SetActive(false);
        if(gameManager.GetComponent<KeyorCont>().keyboard == true){
            keyboard.enabled = true;
            controller.enabled = false;
        }else{
            keyboard.enabled = false;
            controller.enabled = true;
        }
    }

   public void Gameplay()
    {
        ControlsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
        ControlsMenu.SetActive(false);
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7")) && FindObjectOfType<DialogueManager>().dialogueText.enabled == false)
        {
           if(activity == false)
            {
                Pause();
            }
           else if (activity == true)
            {
                Resume();
            }
        }
        

    }

}