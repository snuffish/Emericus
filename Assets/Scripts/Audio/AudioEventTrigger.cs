using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioEventTrigger : MonoBehaviour
{
 public enum Action
    {
        None,
        Play,
        Stop,
        SetParameter
    }

    [System.Serializable]
    public class AudioSettings
    {
        //[HideInInspector]
        public MusicAction action;
        public BackgroundMusicEvents bgmEvent;
        /*public string tag = "";
        public string parameter = "";
        public float targetValue;*/
        public bool ignoreFadeOut;
        public string paramName;
        public float paramValue;
        public bool ignoreSeek;
        public bool paramGlobal;
    }

    [NonReorderable] public AudioSettings[] audioSettings;
    
    private NewAManager aM;
    
    void Start()
    {
        aM = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<NewAManager>();
    }

    public void RunSettings()
    {
        if (audioSettings.Length != 0)
        {
            int number = 1;
            foreach (AudioSettings a in audioSettings)
            {
                /*if ((a.parameter == "" && a.action == Action.SetParameter) || a.tag == "")
                {
                    Debug.Log("You have unfinished fields in AudioTriggerSettings number " + number++);

                }
                else
                {*/
                    switch (a.action)
                    {
                        case MusicAction.None:
                            Debug.Log("Something is Missing!");
                            break;
                        case MusicAction.Play:
                            aM.PlayBGM(a.bgmEvent);
                            Debug.Log("Playing music");
                            break;
                        case MusicAction.Stop:
                            aM.StopBGM(a.bgmEvent, a.ignoreFadeOut);
                            Debug.Log("Stopping music");
                            break;
                        case MusicAction.SetParameter:
                            aM.SetParameterBGM(a.bgmEvent, a.paramName, a.paramValue, a.ignoreSeek, a.paramGlobal);
                            Debug.Log("Setting parameter");
                            break;
                        default: Debug.Log("Error. No valid action.");
                            break;
                    }
                    //number++;
                //}  
            }
        }
        else
            Debug.Log("AudioTriggerSettings was NULL");
    }
}
