using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioEventSettings : MonoBehaviour
{
    [System.Serializable]
    public struct AudioSettings
    {
        public MusicAction action;
        public BackgroundMusicEvents bgmEvent;
        public bool ignoreFadeOut;
        public string paramName;
        public float paramValue;
        public bool ignoreSeek;
        public bool paramIsGlobal;
    }

    [Header("TriggerSettings")] 
    [NonReorderable] public AudioSettings[] audioSettings;

    private NewAManager aM;
    
    private void TriggerAudioSettings()
    {
        aM = GameObject.Find("NewAManager").GetComponent<NewAManager>();
        
        foreach (AudioSettings a in audioSettings)
        {
            switch (a.action)
            {
                case MusicAction.None:
                    Debug.Log("Something is Missing!");
                    break;
                case MusicAction.Play:
                    aM.PlayBGM(a.bgmEvent);
                    break;
                case MusicAction.Stop:
                    aM.StopBGM(a.bgmEvent, a.ignoreFadeOut);
                    break;
                case MusicAction.SetParameter:
                    aM.SetParameterBGM(a.bgmEvent, a.paramName, a.paramValue, a.ignoreSeek, a.paramIsGlobal);
                    break;
                default: Debug.Log("Error. No valid action.");
                    break;
            }
        }
    }
}
