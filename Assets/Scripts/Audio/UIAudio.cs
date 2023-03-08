using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;


public class UIAudio : MonoBehaviour
{
    [SerializeField] EventReference UIClickEventReference;

    private void Start()
    {
        // Get the FMOD event instance
        RuntimeManager.CreateInstance(UIClickEventReference);
    }

    public void OnClickSound() {
        // Play the FMOD event on hover
        RuntimeManager.PlayOneShot(UIClickEventReference);
    }
}