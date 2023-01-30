using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : Interactable
{

    [SerializeField] private bool isOpen;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioSource audioSource;
    
    
    void Start() {
        isOpen = false;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }
    public override void Interact() {

        animator.SetTrigger("Interact");
        audioSource.Play();

    }
}