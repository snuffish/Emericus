using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Oskar Test"); //load whatever scene you want to go to. Either Name or Index
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //or next scene in list
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public GameObject BackButton;
    void start()
    {
        BackButton.SetActive(false);

    }


}
