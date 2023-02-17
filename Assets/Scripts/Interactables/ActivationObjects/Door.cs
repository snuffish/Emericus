using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(HingeJoint))]
[RequireComponent(typeof(PhysicsLocker))]
public class Door : Activators
{
    [Header("Settings")]
    [SerializeField] float motorSpeed = 1f;
    [SerializeField] float motorForce = 15f;


    [Header("Components")]
    [SerializeField] HingeJoint hinge;
    [SerializeField] PhysicsObject physicsObject;
    [SerializeField] Animator animator;
    [SerializeField] EventReference DOOR_Open;
    [SerializeField] EventReference DOOR_Close;
    [SerializeField] EventReference DOOR_Open_Puzzle;

    [Header("Debug")]
    [SerializeField] bool isOpening;
    [SerializeField] bool isClosing;

    void Start()
    {
        if (hinge == null) hinge = GetComponent<HingeJoint>();
        if (physicsObject == null) physicsObject = GetComponent<PhysicsObject>();
        isOpening = false;
        isClosing = false;
    }

    public override void Interact()
    {
        if (!isDisabled)
        {
            InvertState();
        }
    }
    public override void ChangeState(bool toState)
    {
        base.ChangeState(toState);
        if (isActive) //animator.SetBool("IsOpen", true);
        {
            if (!isOpening)
            {
                StartCoroutine(OpenDoor());
                //animator.SetBool("IsOpen", true);
                AudioManager.Instance.PlayOneShot(DOOR_Open_Puzzle, gameObject);
            }
        }
        else
        {
            if (!isClosing) StartCoroutine(CloseDoor());
            //animator.SetBool("IsOpen", false);
        }
    }
    IEnumerator OpenDoor()
    {
        print("Poop");
        physicsObject.UnlockObject();
        isOpening = true;
        isClosing = false;
        while (hinge.angle < hinge.limits.max && !isClosing && !physicsObject.IsLocked)
        {
            physicsObject.UnlockObject();
            hinge.useMotor = true;
            JointMotor motor = hinge.motor;
            motor.targetVelocity = motorSpeed * Mathf.Sign(hinge.limits.max - hinge.angle);
            motor.force = motorForce;
            hinge.motor = motor;
            yield return null;
        }
        hinge.useMotor = false;
        isOpening = false;
    }
    IEnumerator CloseDoor()
    {
        physicsObject.UnlockObject();
        isClosing = true;
        isOpening = false;

        while (hinge.angle > hinge.limits.min && !isOpening && !physicsObject.IsLocked)
        {
            hinge.useMotor = true;
            JointMotor motor = hinge.motor;
            motor.targetVelocity = motorSpeed * Mathf.Sign(hinge.limits.min - hinge.angle);
            motor.force = motorForce;
            hinge.motor = motor;
            yield return null;
        }
        hinge.useMotor = false;
        isClosing = false;
    }

}