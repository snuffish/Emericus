using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
[RequireComponent(typeof(PhysicsLocker))]
public class Lever : Activators
{
    [Header("Settings")]
    [SerializeField, Tooltip("Resend Activation/deactivation Inputs to reciever every frame when enabled")] bool activateOnUpdate = false;
    [SerializeField] float motorSpeed = 1f;
    [SerializeField] float motorForce = 15f;
    [SerializeField] float activateAngle = 50f;
    [SerializeField] float deactivateAngle = 0f;
    [SerializeField] float activateThreshold = 3f;
    [Header("Components")]
    [SerializeField] HingeJoint hinge;
    [SerializeField] PhysicsObject physicsObject;

    [Header("Debug")]
    [SerializeField] bool isActivating;
    [SerializeField] bool isDeactivating;
    void Start()
    {
        if (hinge == null) hinge = GetComponent<HingeJoint>();
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
        if (isActive)
        {
            if (!isActivating)
            {
                StartCoroutine(ActivateLever());
            }
        }
        else
        {
            if (!isDeactivating) StartCoroutine(DeactivateLever());
        }
    }

    void Update()
    {
        if (!isDisabled)
        {
            if (activateOnUpdate)
            {
                if (Mathf.Abs(hinge.angle - deactivateAngle) < activateThreshold)
                {
                    if (isDeactivating) isDeactivating = false;
                    Deactivate();
                }
                if (Mathf.Abs(hinge.angle - activateAngle) < activateThreshold)
                {
                    if (isActivating) isActivating = false;
                    Activate();
                }
            }
            else
            {
                if (isActive)
                {
                    if (Mathf.Abs(hinge.angle - deactivateAngle) < activateThreshold)
                    {
                        if (isDeactivating) isDeactivating = false;
                        Deactivate();
                    }
                }
                if (!isActive)
                {
                    if (Mathf.Abs(hinge.angle - activateAngle) < activateThreshold)
                    {
                        if (isActivating) isActivating = false;
                        Activate();
                    }
                }
            }
        }
    }
    public IEnumerator ActivateLever()
    {
        physicsObject.UnlockObject();
        isActivating = true;
        isDeactivating = false;
        while (hinge.angle < hinge.limits.max && !isDeactivating && !physicsObject.IsLocked)
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
        isActivating = false;
    }
    IEnumerator DeactivateLever()
    {
        physicsObject.UnlockObject();
        isDeactivating = true;
        isActivating = false;
        while (hinge.angle > hinge.limits.min && !isActivating && !physicsObject.IsLocked)
        {
            hinge.useMotor = true;
            JointMotor motor = hinge.motor;
            motor.targetVelocity = motorSpeed * Mathf.Sign(hinge.limits.min - hinge.angle);
            motor.force = motorForce;
            hinge.motor = motor;
            yield return null;
        }
        hinge.useMotor = false;
        isDeactivating = false;
    }
}