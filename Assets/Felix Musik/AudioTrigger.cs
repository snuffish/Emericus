using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
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
        public StudioEventEmitter sEmitter;
        public Action action = Action.None;
        public string paramName = "";
        public float paramValue = 0;
        public bool ignoreSeek = false;
    }
    
    [SerializeField] private bool destroyTrigger = false;
    [SerializeField] private GameObject gObject;

    public AudioManager audioManager;
    
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
                        case Action.Play:
                            audioManager.Play(i.sEmitter);
                            break;
                        case Action.Stop:
                            audioManager.Stop(i.sEmitter);
                            break;
                        case Action.SetParameter:
                            audioManager.SetParameter(i.sEmitter, i.paramName, i.paramValue, false);
                            break;
                        default:
                            Debug.Log("Error. No valid MusicTrigger value");
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
