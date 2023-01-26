using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Door : Interactable
{

    [SerializeField] private bool isOpen;
    [SerializeField] private Animator animator;
    

    void Start() {
        isOpen = false;
    }
    public override void Interact() {
        
        animator.SetTrigger("Interact");
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);

    }
}
