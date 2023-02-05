using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : Interactable
{
    float waitOnPickup = 0.2f;
    [SerializeField, Tooltip("Amount of Force before an object is dropped")] float breakForce = 35f;
    [SerializeField] public PlayerInteract playerInteract;
    [SerializeField] float stackNormalThreshold = 0.5f;
    //[SerializeField] float throwForce = 10;
    //[SerializeField] float damageModifier = 1;
    [HideInInspector] public bool pickedUp = false;
    float baseWeight;
    Dictionary<GameObject, float> stackedObjects = new Dictionary<GameObject, float>();

    Rigidbody rBody;

    void Start()
    {
        playerInteract = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteract>();
        rBody = GetComponent<Rigidbody>();
        baseWeight = rBody.mass;
    }
    public override void Interact()
    {
        if (!pickedUp) StartCoroutine(PickUp());
        else
        {
            pickedUp = false;
            playerInteract.BreakConnection();
        }
    }
    void Update()
    {
        if (stackedObjects.Count <= 0) rBody.mass = baseWeight;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (collision.contacts[0].normal.y < -stackNormalThreshold)
            {
                float collisionObjectMass = collision.rigidbody.mass;
                rBody.mass += collisionObjectMass;
                stackedObjects.Add(collision.gameObject, collisionObjectMass);
            }

            if (pickedUp)
            {
                if (collision.relativeVelocity.magnitude > breakForce)
                {
                    playerInteract.BreakConnection();
                }
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
                if (!stackedObjects.ContainsKey(collision.gameObject))
                {
                    stackedObjects.Add(collision.gameObject, collisionObjectMass);
                    rBody.mass += collisionObjectMass;
                }
                else
                {
                    if (stackedObjects.ContainsKey(collision.gameObject))
                    {
                        float previousMass = stackedObjects[collision.gameObject];
                        rBody.mass -= previousMass;
                        stackedObjects[collision.gameObject] = collisionObjectMass;
                        rBody.mass += collisionObjectMass;
                    }
                }
            }
            
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (stackedObjects.ContainsKey(collision.gameObject))
            {
                rBody.mass -= stackedObjects[collision.gameObject];
                stackedObjects.Remove(collision.gameObject);
            }
        }
    }

    public IEnumerator PickUp()
    {
        yield return new WaitForSecondsRealtime(waitOnPickup);
        pickedUp = true;

    }
}
