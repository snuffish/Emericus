using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioZone : MonoBehaviour
{
    public enum ZoneAction
    {
        None,
        Play,
        Stop,
        SetParameter
    }

    [System.Serializable]

    public struct ZoneSettings
    {
        public ZoneAction enter;
        public StudioEventEmitter sEmitter;
        public string paramName;
        public float paramValue;
        public bool ignoreSeek;
    }

    [SerializeField] private GameObject player;

    public AudioManager audioManager;

    [NonReorderable] public ZoneSettings[] audioSettings;

    void Start()
    {
        foreach (ZoneSettings i in audioSettings)
        {
            switch (i.enter)
            {
                case ZoneAction.Play:
                    audioManager.Play(i.sEmitter);
                    Debug.Log("Play sounds");
                    break;
                case ZoneAction.Stop:
                    audioManager.Stop(i.sEmitter);
                    break;
                case ZoneAction.SetParameter:
                    audioManager.SetParameter(i.sEmitter, i.paramName, i.paramValue, false);
                    break;
                default:
                    Debug.Log("Error! No valid value");
                    break;
            }
        }
    }
}
