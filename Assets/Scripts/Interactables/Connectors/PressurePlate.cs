using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Connector
{
    [SerializeField, Tooltip("Minimum Weight before activated")] float weightThreshold;
    [SerializeField, Tooltip("Max allowed angle(kinda)to be considered stacked")] float stackNormalThreshold = 0.5f;
    //[SerializeField] LayerMask pressureLayers;
    Dictionary<GameObject, float> pressuringObjects = new Dictionary<GameObject, float>();
    float currentMass;
    bool isActive = false;

    public override void Interact()
    {
        
    }

    void Update()
    {
        if (currentMass >= weightThreshold && !isActive)
        {
            //It activates here, insert sounds
            isActive = true;
            foreach (Interactable interactable in connectedToList)
            {
                //Activates every attached object
                interactable.Interact();
            }
        }
        else if (currentMass < weightThreshold && isActive)
        {
            isActive = false;
            foreach (Interactable interactable in connectedToList)
            {
                interactable.Interact();
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (collision.contacts[0].normal.y < -stackNormalThreshold)
            {
                float collisionObjectMass = collision.rigidbody.mass;
                currentMass += collisionObjectMass;
                pressuringObjects.Add(collision.gameObject, collisionObjectMass);

            }
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            float collisionObjectMass = collision.gameObject.GetComponent<Rigidbody>().mass;
            if (collision.contacts[0].normal.y < -stackNormalThreshold)
            {
                if (!pressuringObjects.ContainsKey(collision.gameObject))
                {
                    pressuringObjects.Add(collision.gameObject, collisionObjectMass);
                    currentMass += collisionObjectMass;
                }
                else
                {
                    if (pressuringObjects.ContainsKey(collision.gameObject))
                    {
                        float previousMass = pressuringObjects[collision.gameObject];
                        currentMass -= previousMass;
                        pressuringObjects[collision.gameObject] = collisionObjectMass;
                        currentMass += collisionObjectMass;
                    }
                }
            }

        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (pressuringObjects.ContainsKey(collision.gameObject))
            {
                currentMass -= pressuringObjects[collision.gameObject];
                pressuringObjects.Remove(collision.gameObject);
            }

        }
    }

    //void OnTriggerEnter(Collider collider)
    //{
    //    if(collider)
    //    if(collider.GetComponent<Rigidbody>().mass > weightThreshold)
    //    {
    //        foreach (Interactable interactable in connectedToList)
    //        {
    //            interactable.Interact();
    //        }
    //    }
    //}

    //void OnTriggerExit(Collider collider)
    //{
    //    if (connectedTo.GetComponent<Door>() != null)
    //    {
    //        connectedTo.GetComponent<Door>().SetDoorOpen(false);
    //    }
    //}
}