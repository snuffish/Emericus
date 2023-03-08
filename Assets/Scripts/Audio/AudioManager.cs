using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    // public PlayerAudio playerAudio;

    //Bankladdare
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        BankLoader();
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    [Header("BankRef")]
    [BankRef] public string masterBank;

    public bool loadSampleData;

    private void BankLoader()
    {
        //Debug.Log("Has the bank loaded? " + RuntimeManager.HasBankLoaded(masterBank));
        RuntimeManager.LoadBank(masterBank, loadSampleData);
    }


    //Emitters för bakgrundsmusik
    [Header("Voicelines")]
    public Emitters eventEmitters;

    [System.Serializable]

    public struct Emitters
    {
        public StudioEventEmitter painting;
        public StudioEventEmitter statue;
        public StudioEventEmitter tidBit1;
        public StudioEventEmitter tidBit2;
        public StudioEventEmitter uiClick;
    }


    public void PlayFootstep()
    {
        RuntimeManager.PlayOneShot(playerFootsteps);
    }



    //Player Funktioner Events.
    [Header("Player")]
    [SerializeField] private EventReference playerFootsteps;

    /*
    [EventRef]
    public string playerFootsteps;
    EventInstance footstepInstance;
    */

    //Play, Stop, Parameter
    public void Play(StudioEventEmitter emitter)
    {
        if (emitter.IsActive == false)
        {
            emitter.Play();
            Debug.Log("Playing!");
        }
        else
        {
            Debug.Log("Already playing!");
        }
    }

    public void PlayOneShot(EventReference eventRef, GameObject soundObject)
    {
        RuntimeManager.PlayOneShotAttached(eventRef.Guid, soundObject);
    }
    public void Stop(StudioEventEmitter emitter)
    {
        emitter.Stop();
        Debug.Log("Shut up!");
    }

    public void playOneShotWithParameter(EventReference eventRef, string paramName, float paramValue, bool ignorSeek, GameObject soundObject)
    {
        RuntimeManager.PlayOneShotAttached(eventRef.Guid, soundObject);
        //RuntimeManager.StudioSystem.setParameterByName(paramName, paramValue, ignorSeek);
        //.SetParameter(paramName,);
    }
    
    public void SetParameter(StudioEventEmitter emitter, string paramName, float paramValue, bool ignorSeek)
    {
        RuntimeManager.StudioSystem.setParameterByName(paramName, paramValue, ignorSeek);
        Debug.Log("Setting parameter.");
    }
}
