using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralPlayerInputs : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private bool gameIsPaused = false;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("PauseButton")) {
            //  Unpause the game
            if (gameIsPaused) {
                gameIsPaused = false;
                Cursor.visible = false;
                pauseMenu.active = false;
                Time.timeScale = 1;
            }
            
            //  Pause the game
            else {
                gameIsPaused = true;
                Cursor.visible = true;
                pauseMenu.active = true;
                Time.timeScale = 0;
            }
        }
    }
}