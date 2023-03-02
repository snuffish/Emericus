using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class VLTrigger : MonoBehaviour
{
    public string requiredTag = "Player";
    public bool destroyAfterUse = true;

    [System.Serializable]

    public struct VoiceSettings
    {
        public VoiceAction vAction;
        public VoiceEvent vEvent;
        public string keyName;

    }

    [Header("TriggerSettings")] 
    [NonReorderable] public VoiceSettings[] voiceSettings;
    
    private VLManager vM;

    void Start()
    {
        vM = GameObject.Find("VLManager").GetComponent<VLManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag != requiredTag)
            return;

        foreach (VoiceSettings v in voiceSettings)
        {
            switch (v.vAction)
            {
               case VoiceAction.PlayDialogue:
                   vM.PlayDialogue(v.vEvent, v.keyName);
                   vM.dialogueInstance.start();
                   break;
               default:Debug.Log("Error! No valid action.");
                   break;
            }
        }
        
        
        if (destroyAfterUse == true)
        {
            Destroy(gameObject);
        }
    }
    

}
