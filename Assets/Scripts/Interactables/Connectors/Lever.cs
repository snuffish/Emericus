using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Connector
{
    public override void Interact() {
        foreach (Interactable interactable in connectedToList)
        {
            interactable.Interact();
        }
    }
}