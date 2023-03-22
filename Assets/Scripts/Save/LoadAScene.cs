using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadAScene : Interactable
{
    /*[FMODUnity.ParamRef] 
    public string paramRef;

    public float paramValue;
    public bool ignoreSeek = false;*/
    
    public BackgroundMusicEvents bgmEvent;
    public bool ignoreFadeOut;
   /* public VoiceEvent vEvent;
    public string key;*/
    
    [SerializeField] private SceneManager _sceneManager;
    [SerializeField] private float transistionTime;
    [SerializeField] private int sceneToLoadInt;

    public override void Interact() {
        GameObject.FindGameObjectWithTag("MusicManager").GetComponent<NewAManager>().StopBGM(bgmEvent, ignoreFadeOut);
        //GameObject.FindGameObjectWithTag("VLManager").GetComponent<VLManager>().StopDialogue(vEvent, key);
        
        //FMODUnity.RuntimeManager.StudioSystem.setParameterByName(paramRef, paramValue, ignoreSeek);
        StartCoroutine(_sceneManager.LoadLevel(sceneToLoadInt, transistionTime));
    }
}