using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhysicsObject))]
[RequireComponent(typeof(HingeJoint))]

public class PhysicsLocker : MonoBehaviour
{
    [SerializeField] List<float> lockAngles = new List<float>();
    [SerializeField] HingeJoint hinge;
    [SerializeField] float lockThreshold = 2f;
    [SerializeField] float lockBreakThreshold = 10f;
    [SerializeField] bool targetIsLocked = false;
    [SerializeField] bool forceCanBreakLock = false;
    [SerializeField] Rigidbody rigidbodyTarget;
    public PhysicsObject targetPhysicsObject;

    void Start()
    {
        if (rigidbodyTarget == null) rigidbodyTarget = GetComponent<Rigidbody>();
        if (targetPhysicsObject == null) targetPhysicsObject = GetComponent<PhysicsObject>();
        if (hinge == null) hinge = GetComponent<HingeJoint>();
        targetIsLocked = targetPhysicsObject.IsLocked;
    }
    void Update()
    {
        targetIsLocked = targetPhysicsObject.IsLocked;
        if (!targetIsLocked)
        {
            if (targetPhysicsObject.pickedUp == false)
            {
                foreach (float angle in lockAngles)
                {
                    if (Mathf.Abs(hinge.angle - angle) < lockThreshold)
                    {
                        targetPhysicsObject.LockObject();
                        break;
                    }
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (forceCanBreakLock)
        {
            if (collision.relativeVelocity.magnitude > lockBreakThreshold) targetPhysicsObject.UnlockObject();
        }
    }
}
