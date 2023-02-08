using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : Activators
{
    [SerializeField] private bool isOpen;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioSource audioSource;


    void Start()
    {
        isOpen = false;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }

    public override void Interact()
    {
        if (!isLocked)
        {
            InvertState();
        }
    }
    public override void ChangeState(bool toState)
    {
        base.ChangeState(toState);
        if(isActive) animator.SetBool("IsOpen", true);
        else animator.SetBool("IsOpen", false);

    }
    //void Update()
    //{
    //    if (isActive)
    //    {
    //        animator.SetBool("IsOpen", true);
    //    }
    //    else
    //    {
    //        animator.SetBool("IsOpen", false);
    //    }
    //}
    
}