using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioTrigger : MonoBehaviour
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

    [SerializeField] private bool destroyTrigger = false;
    [SerializeField] private GameObject gObject;

    public AudioManager audioManager;
    public MusicManager musicManager;

    [NonReorderable] public TriggerSettings[] audioSettings;

    void OnTriggerEnter(Collider other)
    {
        foreach (TriggerSettings i in audioSettings)
        {
            if (other.gameObject == gObject)
            {
                gObject = other.gameObject;
                
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
                
                if (destroyTrigger == true)
                {
                    Destroy(gameObject.GetComponent<BoxCollider>());
                    Debug.Log("Trigger is no more!");
                }
            }
        }
    }
}
