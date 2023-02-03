using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : Interactable
{
    float waitOnPickup = 0.2f;
    [SerializeField, Tooltip("Amount of Force before an object is dropped")] float breakForce = 35f;
    [SerializeField] float throwForce = 10;
    [SerializeField] float damageModifier = 1;
    [SerializeField] public PlayerInteract playerInteract;
    [HideInInspector] public bool pickedUp = false;
    Rigidbody rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        playerInteract = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteract>();
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
    void OnCollisionEnter(Collision collision)
    {
        if (pickedUp)
        {
            if (collision.relativeVelocity.magnitude > breakForce)
            {
                playerInteract.BreakConnection();
            }
        }
    }

    public IEnumerator PickUp()
    {
        yield return new WaitForSecondsRealtime(waitOnPickup);
        pickedUp = true;

    }
}
