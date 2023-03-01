using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public enum MusicAction
{
    None,
    Play,
    Stop,
    SetParameter
}

public enum BackgroundMusicEvents
{
    None,
    BGM1,
    BGM2,
    SafeRoom
}
public class NewAManager : MonoBehaviour
{
    public static NewAManager Instance;

    [Header("Background Music")] 
    [SerializeField] private EventReference[] bgmReferences = new EventReference[3];
    private EventInstance[] bgmInstances = new EventInstance[3];

    [Header("GameOver")] 
    [SerializeField] private EventReference gameOverStinger;

    private void Awake()
    {
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

    public void PlayBGM(BackgroundMusicEvents bgmEvent)
    {
        int num = Convert.ToInt32(bgmEvent) - 1;                
        //-1 för att det finns 4 val BGM enumen men bara 3 instancer

        if (num < 0)
        {
            Debug.Log("INVALID EVENT CHOSEN!");
            return;
        }

        bool isActive = CheckActiveState(bgmInstances[num]);

        if (!isActive)
        {
            Debug.Log("Event not active before. Activating");
            bgmInstances[num] = RuntimeManager.CreateInstance(bgmReferences[num]);
            bgmInstances[num].start();
        }
        
        
    }
    
    public void StopBGM(BackgroundMusicEvents bgmEvent, bool ignoreFadeOut)
    {
        int num = Convert.ToInt32(bgmEvent) - 1;

        if (num < 0)
        {
            Debug.Log("INVALID EVENT CHOSEN!");
            return;
        }

        if (ignoreFadeOut)
        {
           bgmInstances[num].stop(STOP_MODE.IMMEDIATE); 
        }
        else
        {
            bgmInstances[num].stop(STOP_MODE.ALLOWFADEOUT);
        }
        
        bgmInstances[num].release();
    }

    public void SetParameterBGM(BackgroundMusicEvents bgmEvent, string paramName,
                                float paramValue, bool ignoreSeek, bool globalParam)
    {
        if (globalParam)
        {
            RuntimeManager.StudioSystem.setParameterByName(paramName, paramValue, ignoreSeek);
            return;
        }
        
        int num = Convert.ToInt32(bgmEvent) - 1;

        if (num < 0)
        {
            Debug.Log("INVALID EVENT CHOSEN!");
            return;
        }

        bgmInstances[num].setParameterByName(paramName, paramValue, ignoreSeek);
    }

    private bool CheckActiveState(EventInstance eInstance)
    {
        bool isActive = true;

        eInstance.getPlaybackState(out PLAYBACK_STATE state);

        if (state == PLAYBACK_STATE.STOPPED || state == PLAYBACK_STATE.STOPPING)
        {
            isActive = false;
        }

        return isActive;
    }

    public void PlayGameOver()
    {
        RuntimeManager.PlayOneShot(gameOverStinger);
    }
}
