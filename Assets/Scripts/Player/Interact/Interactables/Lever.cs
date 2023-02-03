using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{

    [SerializeField] private Interactable connectedTo;
    

    public override void Interact() {
        connectedTo.Interact();
    }
}