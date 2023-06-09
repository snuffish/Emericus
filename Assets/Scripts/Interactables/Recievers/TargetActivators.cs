using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TargetActivators : Interactable
{
    [SerializeField] protected float activationForce = 30f;
    [SerializeField] protected bool invertOnPress = true;
    [SerializeField] protected bool deactivateOnPress = false;
    [SerializeField] protected bool activateOnPress = false;

    [SerializeField] protected List<Activators> targets = new List<Activators>();
    [SerializeField] EventReference buttonSound;

    public override void Interact()
    {
        foreach (Activators target in targets)
        {
            if (invertOnPress) target.InvertState();
            else if (deactivateOnPress) target.Deactivate();
            else if (activateOnPress) target.Activate();
            else print("Please select a way for targetActivator to interact");
        }
        AudioManager.Instance.PlayOneShot(buttonSound, gameObject);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > activationForce) Interact();
    }
}
