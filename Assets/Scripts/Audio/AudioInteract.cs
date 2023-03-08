using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioInteract : Interactable
{
    public enum TriggerAction
    {
        None,
        Play,
        Stop,
        SetParameter
    }

    [System.Serializable]

    public struct TriggerSettings
    {
        public TriggerAction enter;
        public StudioEventEmitter sEmitter;
        public string paramName;
        public float paramValue;
    }

    [SerializeField] private GameObject gObject;

    public override void Interact()
    {
        foreach (TriggerSettings i in audioSettings)
        {
            switch (i.enter)
            {
                case TriggerAction.Play:
                    audioManager.Play(i.sEmitter);
                    Debug.Log("Play sounds");
                    break;
                case TriggerAction.Stop:
                    audioManager.Stop(i.sEmitter);
                    break;
                case TriggerAction.SetParameter:
                    audioManager.SetParameter(i.sEmitter, i.paramName, i.paramValue, false);
                    break;
                default:
                    Debug.Log("Error! No valid value");
                    break;
            }
        }
    }

    public AudioManager audioManager;
    public NewAManager musicManager;

    [NonReorderable] public TriggerSettings[] audioSettings;

    void OnTriggerEnter(Collider other)
    {

    }
}
