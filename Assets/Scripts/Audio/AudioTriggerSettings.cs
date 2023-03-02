using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioTriggerSettings : MonoBehaviour
{
    public string requiredTag = "Player";
    public bool destroyAfterUse = false;
    
    [System.Serializable]
    
    public struct AudioSettings
    {
        public MusicAction action;
        public BackgroundMusicEvents bgmEvent;
        public bool ignoreFadeOut;
        public string paramName;
        public float paramValue;
        public bool ignoreSeek;
        public bool paramGlobal;
    }

    [Header("TriggerSettings")] 
    [NonReorderable] public AudioSettings[] audioSettings;

    private NewAManager aM;

    void Start()
    {
        aM = GameObject.Find("NewAManager").GetComponent<NewAManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != requiredTag)
            return;

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
                    aM.SetParameterBGM(a.bgmEvent, a.paramName, a.paramValue, a.ignoreSeek, a.paramGlobal);
                    Debug.Log("Setting parameter");
                    break;
                default: Debug.Log("Error. No valid action.");
                    break;
            }
        }

        if (destroyAfterUse == true)
        {
            Destroy(gameObject);
        }
    }
}
