using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : Interactable
{
    float waitOnPickup = 0.2f;
    public float breakForce = 35f;
    [SerializeField] float throwForce = 10;
    [SerializeField] float damageModifier = 1;
    [SerializeField] public TempPlayerInteract tempPlayerInteract;
    [HideInInspector] public bool pickedUp = false;
    Rigidbody rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }
    public override void Interact()
    {
        if (!pickedUp) StartCoroutine(PickUp());
        else
        {
            pickedUp = false;
            tempPlayerInteract.BreakConnection();
        }
    }
    void Update()
    {
    }

    public IEnumerator PickUp()
    {
        yield return new WaitForSecondsRealtime(waitOnPickup);
        pickedUp = true;

    }
}
