using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;

public enum VoiceAction
{
    None,
    PlayDialogue
}

public enum VoiceEvent
{
    Dialogue
}

public class VLManager : MonoBehaviour
{
   //VLProgrammer
    private EVENT_CALLBACK dialogueCallback;

    public EventReference EventName;
    
    public string requiredTag = "Player";
    //public bool destroyAfterUse = true;
    //public string keyName;
    private static int subtitleCount = 0;
    
    //TimelineCallback
    class TimelineInfo
    {
        public int CurrentMusicBar = 0;
        public FMOD.StringWrapper LastMarker = new FMOD.StringWrapper();
    }

    TimelineInfo timelineInfo;
    GCHandle timelineHandle;

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
    }
    
    //TimelineCallback
       void OnDestroy()
    {
        dialogueInstance.setUserData(IntPtr.Zero);
        dialogueInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        dialogueInstance.release();
        timelineHandle.Free();
    }

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
                        Debug.Log($"start voiceline {subtitleCount}");
                        subtitleCount++;
                    }
                    break;
            }
        }
        return FMOD.RESULT.OK;
    }
    
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
