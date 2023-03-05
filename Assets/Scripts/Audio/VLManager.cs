using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;
using TMPro;

public enum VoiceAction
{
    None,
    PlayDialogue,
    //SetParameter
}

public enum VoiceEvent
{
    Dialogue
}

public class VLManager : MonoBehaviour
{
    [SerializeField] private List<string> voiceLineMarkers;
    [SerializeField] private List<string> voiceLines;
    private static Dictionary<string, string> voiceLineDictionary;
    [SerializeField] private TMP_Text subtitleField;
    static private string subtitleText;


    [Header("Dialogue")] 
    /*[SerializeField] private EventReference[] vLReferences = new EventReference[1];
    private EventInstance[] bgmInstances = new EventInstance[1];*/
    
   //VLProgrammer
    private EVENT_CALLBACK dialogueCallback;

    //EmitterTest
    /*public VLEmitter vEmitter;
        
    public struct VLEmitter
    {
        public StudioEventEmitter dialogue1;
    }*/

    public EventReference EventName;
    
    //public string requiredTag = "Player";
    //public bool destroyAfterUse = true;
    //public string keyName;

    //TimelineCallback
    
    class TimelineInfo
    {
        public int CurrentMusicBar = 0;
        public FMOD.StringWrapper LastMarker = new FMOD.StringWrapper();
    }

    TimelineInfo timelineInfo;
    public GCHandle timelineHandle;

    EVENT_CALLBACK beatCallback;
    public EventInstance dialogueInstance;
    
    //Båda
#if UNITY_EDITOR
    void Reset()
    {
        EventName = EventReference.Find("event:Voicelines/Dialogue");
    }
#endif
    
    void Start()
    {
        //VLProgrammer
        dialogueCallback = new EVENT_CALLBACK(DialogueEventCallback);
        
        //TimelineCallback
        timelineInfo = new TimelineInfo();

        // Explicitly create the delegate object and assign it to a member so it doesn't get freed
        // by the garbage collected while it's being used
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);

        dialogueInstance = FMODUnity.RuntimeManager.CreateInstance(EventName);

        // Pin the class that will store the data modified during the callback
        timelineHandle = GCHandle.Alloc(timelineInfo);
        // Pass the object through the userdata of the instance
        dialogueInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));

        dialogueInstance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        //dialogueInstance.start();
        
        voiceLineDictionary = new Dictionary<string, string>();
        for (int i = 0; i < voiceLineMarkers.Count; i++) {
            voiceLineDictionary.Add(voiceLineMarkers[i], voiceLines[i]);
        }
    }

    void Update() {
        subtitleField.text = subtitleText;
    }
    
    //TimelineCallback
    /*
    void OnDestroy()
    {
        dialogueInstance.setUserData(IntPtr.Zero);
        dialogueInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        dialogueInstance.release();
        timelineHandle.Free();
    }
*/
    void OnGUI()
    {
        GUILayout.Box(String.Format("Current Bar = {0}, Last Marker = {1}", timelineInfo.CurrentMusicBar, (string)timelineInfo.LastMarker));
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

        // Retrieve the user data
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            // Get the object to store beat and marker details
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.CurrentMusicBar = parameter.bar;
                    }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.LastMarker = parameter.name;
                        subtitleText = voiceLineDictionary[(string) timelineInfo.LastMarker];
                    }
                    break;
            }
        }
        return FMOD.RESULT.OK;
    }

    /*public void SetParameterVL(VoiceEvent vEvent, string paramName, float paramValue, bool ignoreSeek, bool globalParam)
    {
        if (globalParam)
        {
            RuntimeManager.StudioSystem.setParameterByName(paramName, paramValue, ignoreSeek);
            return;
        }
        
        int num = Convert.ToInt32(vEvent) - 1;

        if (num < 0)
        {
            Debug.Log("INVALID EVENT CHOSEN!");
            return;
        }

        //bgmInstances[num].setParameterByName(paramName, paramValue, ignoreSeek);
    }*/
    
    //VLProgrammer
        public void PlayDialogue(VoiceEvent vEvent, string key)
    {
        var dialogueInstance = RuntimeManager.CreateInstance(EventName);

        GCHandle stringHandle = GCHandle.Alloc(key);
        dialogueInstance.setUserData(GCHandle.ToIntPtr(stringHandle));

        dialogueInstance.setCallback(dialogueCallback);
        dialogueInstance.start();
        dialogueInstance.release();
    }
        

    [AOT.MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
    static FMOD.RESULT DialogueEventCallback(EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        EventInstance instance = new EventInstance(instancePtr);

        //Retriev the user data       
        IntPtr stringPtr;
        instance.getUserData(out stringPtr);
        
        //Get the string object
        GCHandle stringHandle = GCHandle.FromIntPtr(stringPtr);
        String key = stringHandle.Target as String;

        switch (type)
        {
            case EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND:
            {
                FMOD.MODE soundMode = FMOD.MODE.LOOP_NORMAL | FMOD.MODE.CREATECOMPRESSEDSAMPLE | FMOD.MODE.NONBLOCKING;
                var parameter = (PROGRAMMER_SOUND_PROPERTIES) Marshal.PtrToStructure(parameterPtr,
                    typeof(PROGRAMMER_SOUND_PROPERTIES));
                if (key.Contains("."))
                {
                    FMOD.Sound dialogueSound;
                    var soundResult = RuntimeManager.CoreSystem.createSound(Application.streamingAssetsPath + "/" +
                                                                            key, soundMode, out dialogueSound);
                    if (soundResult == FMOD.RESULT.OK)
                    {
                        parameter.sound = dialogueSound.handle;
                        parameter.subsoundIndex = -1;
                        Marshal.StructureToPtr(parameter, parameterPtr, false);
                    }
                }
                else
                {
                    SOUND_INFO dialogueSoundInfo;
                    var keyResult = RuntimeManager.StudioSystem.getSoundInfo(key, out dialogueSoundInfo);
                    if (keyResult != FMOD.RESULT.OK)
                    {
                        break;
                    }

                    FMOD.Sound dialogueSound;
                    var soundResult = RuntimeManager.CoreSystem.createSound(dialogueSoundInfo.name_or_data,
                        soundMode | dialogueSoundInfo.mode, ref dialogueSoundInfo.exinfo, out dialogueSound);
                    if (soundResult == FMOD.RESULT.OK)
                    {
                        parameter.sound = dialogueSound.handle;
                        parameter.subsoundIndex = dialogueSoundInfo.subsoundindex;
                        Marshal.StructureToPtr(parameter, parameterPtr, false);
                    }
                }

                break;
            }
            case EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND:
            {
                var parameter = (PROGRAMMER_SOUND_PROPERTIES) Marshal.PtrToStructure(parameterPtr,
                    typeof(PROGRAMMER_SOUND_PROPERTIES));
                var sound = new FMOD.Sound(parameter.sound);
                sound.release();
                
                break;
            }
            case EVENT_CALLBACK_TYPE.DESTROYED:
            {
                stringHandle.Free();
                
                break;
            }
        }

        return FMOD.RESULT.OK;
    }
}
