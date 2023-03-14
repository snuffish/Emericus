using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadAScene : Interactable
{
    [FMODUnity.ParamRef] 
    public string paramRef;

    public float paramValue;
    public bool ignoreSeek = false;
    [SerializeField] private SceneManager _sceneManager;
    [SerializeField] private float transistionTime;
    [SerializeField] private int sceneToLoadInt;

    public override void Interact() {
        
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(paramRef, paramValue, ignoreSeek);
        StartCoroutine(_sceneManager.LoadLevel(sceneToLoadInt, transistionTime));
    }
}