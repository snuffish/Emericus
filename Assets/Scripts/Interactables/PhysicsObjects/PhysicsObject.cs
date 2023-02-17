using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsSounds))]
public class PhysicsObject : Interactable
{
    float waitOnPickup = 0.2f;
    [SerializeField, Tooltip("Amount of Force before an object is dropped")] float breakForce = 35f;
    [SerializeField] public PlayerInteract playerInteract;
    [SerializeField] float stackNormalThreshold = 0.5f;
    [SerializeField] float lockCooldown = 0.1f;
    public bool canBeRotated = true;
    public bool alwaysLockOnRelease;
    public bool keepRestraints = false;
    RigidbodyConstraints baseConstraints;
    public bool IsLocked
    {
        get { return isLocked; }
        set
        {
            isLocked = value;
        }
    }
    bool isLocked;
    bool canBeLocked = true;

    public PhysicsSounds objectSoundController;
    //[SerializeField] float throwForce = 10;
    //[SerializeField] float damageModifier = 1;
    [HideInInspector] public bool pickedUp = false;
    float baseWeight;
    Dictionary<GameObject, float> stackedObjects = new Dictionary<GameObject, float>();

    Rigidbody rBody;

    void Start()
    {
        if (objectSoundController == null)
            objectSoundController = GetComponent<PhysicsSounds>();
        playerInteract = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteract>();
        rBody = GetComponent<Rigidbody>();
        baseWeight = rBody.mass;
        if (keepRestraints) baseConstraints = rBody.constraints;
        objectSoundController = GetComponent<PhysicsSounds>();
        canBeLocked = true;
    }
    public override void Interact()
    {
        if (!pickedUp)
        {
            StartCoroutine(PickUp());
        }
        else
        {
            pickedUp = false;
            //objectSoundController.DropEvent();
            playerInteract.BreakConnection();
        }
    }
    void Update()
    {
        if (stackedObjects.Count <= 0) rBody.mass = baseWeight;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (objectSoundController != null)
            objectSoundController.CollisionEvent();

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
                    // objectSoundController.DropEvent();
                    playerInteract.BreakConnection();
                }
            }
        }

    }

    void OnCollisionStay(Collision collision)
    {
        //active Collision
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


    public void LockObject()
    {
        if (canBeLocked || alwaysLockOnRelease)
        {
            rBody.constraints = RigidbodyConstraints.FreezePosition;
            isLocked = true;
        }
    }
    public void UnlockObject()
    {
        isLocked = false;
        rBody.constraints = baseConstraints;
        StartCoroutine(LockCooldown());

    }
    public void PlayDropSound()
    {
        // objectSoundController.DropEvent();

    }
    IEnumerator LockCooldown()
    {
        canBeLocked = false;
        yield return new WaitForSeconds(lockCooldown);
        canBeLocked = true;
    }
    public IEnumerator PickUp()
    {
        objectSoundController.PickUpEvent();
        yield return new WaitForSecondsRealtime(waitOnPickup);
        pickedUp = true;
        UnlockObject();

    }
}
