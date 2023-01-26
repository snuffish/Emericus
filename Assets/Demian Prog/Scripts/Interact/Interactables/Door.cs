using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public override void Interact() {
        if(isActiveAndEnabled)
            gameObject.active = false;
        else {
            gameObject.active = true;
        }
    }
}
