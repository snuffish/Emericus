using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : Interactable
{
    public override void Interact()
    {
        if (!isDisabled)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
}
