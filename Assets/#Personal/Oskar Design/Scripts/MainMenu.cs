using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [FMODUnity.ParamRef] 
    public string paramRef;
    public float paramValue;
    public bool ignoreSeek = false;
    [SerializeField] private GeneralPlayerInputs pauseManager;
    
    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); //load whatever scene you want to go to. Either Name or Index
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //or next scene in list
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(paramRef, paramValue, ignoreSeek);
        pauseManager.UnpauseGame();
    }

    public void BackToMainMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
