using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;
using TMPro;

public class VLTrigger : MonoBehaviour
{
    public string requiredTag = "Player";
    public bool destroyAfterUse = true;

    [System.Serializable]

    public struct VoiceSettings
    {
        public VoiceAction vAction;
        public VoiceEvent vEvent;
        //EmitterTest
        //public StudioEventEmitter vEmitter;
        public string keyName;
        /*public string paramName;
        public float paramValue;
        public bool ignoreSeek;
        public bool paramGlobal;*/
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
               /*case VoiceAction.SetParameter:
                   vM.SetParameterVL(v.vEvent, v.paramName, v.paramValue, v.ignoreSeek, v.paramGlobal);
                   break;*/
               default:Debug.Log("Error! No valid action.");
                   break;
            }
        }
        
        
        if (destroyAfterUse == true)
        {
            Destroy(gameObject);
        }
    }
    
   /* void OnDestroy()
    {
        vM.dialogueInstance.setUserData(IntPtr.Zero);
        vM.dialogueInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        vM.dialogueInstance.release();
        vM.timelineHandle.Free();
    }*/

}
