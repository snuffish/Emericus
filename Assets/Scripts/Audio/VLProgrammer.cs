using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;

public class VLProgrammer : MonoBehaviour
{
    private EVENT_CALLBACK dialogueCallback;

    public EventReference EventName;

#if UNITY_EDITOR
    void Reset()
    {
        EventName = EventReference.Find("event:Voicelines/Dialogue");
    }
#endif

    void Start()
    {
        dialogueCallback = new EVENT_CALLBACK(DialogueEventCallback);
    }

    void PlayDialogue(string key)
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayDialogue("Dia_04_Tidbit1");
        }
       
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayDialogue("Dia_Painting");
        }
    }
}
