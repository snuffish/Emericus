using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class NewAudioTrigger : MonoBehaviour
{
    public enum TriggerAction
    {
        None,
        Play,
        Stop,
        SetParameter
    }

    [System.Serializable]

    public class AudioSettings
    {
        public StudioEventEmitter sEmitter;
        public TriggerAction action = TriggerAction.None;
        public string paramName = "";
        public float paramValue = 0;
        public bool ignoreSeek = false;
    }

    [SerializeField] private bool destroyTrigger = false;
    [SerializeField] private GameObject gObject;

    public AudioManager audioManager;
    public MusicManager musicManager;

    [NonReorderable] public AudioSettings[] audioSettings;

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject == gObject)
        {
            gObject = other.gameObject;

            if (audioSettings.Length != 0)
            {
                int number = 1;

                foreach (AudioSettings i in audioSettings)
                {
                    switch (i.action)
                    {
                        case TriggerAction.Play:
                            musicManager.Play(i.sEmitter);
                            Debug.Log("Play sounds");
                            break;
                        case TriggerAction.Stop:
                            musicManager.Stop(i.sEmitter);
                            break;
                        case TriggerAction.SetParameter:
                            musicManager.SetParameter(i.sEmitter, i.paramName, i.paramValue, false);
                            break;
                        default:
                            Debug.Log("Error! No valid value");
                            break;
                    }
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
