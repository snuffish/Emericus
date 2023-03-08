using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadAScene : Interactable
{
    [SerializeField] private int sceneToLoadInt;

    public override void Interact() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoadInt);
    }
}
