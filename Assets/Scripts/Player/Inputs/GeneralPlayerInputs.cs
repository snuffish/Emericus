using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralPlayerInputs : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private bool gameIsPaused = false;
    [SerializeField] private GameObject crosshairs;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("PauseButton")) {
            
            if (!gameIsPaused) {
                PauseGame();
            }
            else {
                UnpauseGame();
            }
        }
    }

    public void PauseGame() {
        gameIsPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.active = true;
        Time.timeScale = 0;
        crosshairs.SetActive(false);
    }

    public void UnpauseGame() {
        gameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.active = false;
        Time.timeScale = 1;
        crosshairs.SetActive(true);
    }
}