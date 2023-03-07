using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripWire : TargetActivators
{
    [SerializeField] LayerMask triggerLayers;
    [SerializeField] bool destroySelfOnActivate = true;
    [SerializeField] bool hasBeenTriggered = false;
    void Start()
    {
        hasBeenTriggered = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggerLayers == (triggerLayers | (1 << other.gameObject.layer)))
        {
            Interact();
        }
    }

    public override void Interact()
    {
        if (!hasBeenTriggered)
        {
            hasBeenTriggered = true;
            foreach (Activators target in targets)
            {
                if (invertOnPress) target.InvertState();
                else if (deactivateOnPress) target.Deactivate();
                else if (activateOnPress) target.Activate();
                else print("Please select a way for targetActivator to interact");
            }
            if (destroySelfOnActivate) Destroy(gameObject);
        }
    }
}
