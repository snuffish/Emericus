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

        if (isOpen) {
            isOpen = false;
            animator.SetTrigger("CloseDoor");
        }

        else {
            isOpen = true;
            animator.SetTrigger("OpenDoor");
        }

    }
}
