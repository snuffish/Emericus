using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PressButtonMakeSound : Interactable
{

    [SerializeField] private AudioClip audioClip;
    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }
    public override void Interact() {
        audioSource.Play();
    }
}