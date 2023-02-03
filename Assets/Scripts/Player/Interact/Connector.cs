using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : Interactable
{
    [SerializeField] protected List<Interactable> connectedToList;

    public override void Interact()
    {
        foreach (Interactable interactable in connectedToList)
        {
            interactable.Interact();
        }
    }
}
