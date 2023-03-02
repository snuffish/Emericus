using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetActivators : Interactable
{
    [SerializeField] float activationForce = 30f;
    [SerializeField] bool invertOnPress = true;
    [SerializeField] bool deactivateOnPress = false;
    [SerializeField] bool activateOnPress = false;

    [SerializeField] List<Activators> targets = new List<Activators>();

    public override void Interact()
    {
        foreach (Activators target in targets)
        {
            if (invertOnPress) target.InvertState();
            else if (deactivateOnPress) target.Deactivate();
            else if (activateOnPress) target.Activate();
            else print("Please select a way for targetActivator to interact");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > activationForce) Interact();
    }
}
