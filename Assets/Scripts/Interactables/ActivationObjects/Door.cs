using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class Door : Activators
{
    [SerializeField] private bool isOpen;
    [SerializeField] private Animator animator;
    [SerializeField] private EventReference DOOR_Open;
    [SerializeField] private EventReference DOOR_Close;
    [SerializeField] private EventReference DOOR_Open_Puzzle;


    void Start()
    {
        isOpen = false;

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
        if (isActive) //animator.SetBool("IsOpen", true);
        { 
            animator.SetBool("IsOpen", true);
            AudioManager.Instance.PlayOneShot(DOOR_Open_Puzzle, gameObject);
        }
       
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