using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class Lever : Activators
{
    [SerializeField] float activateAngle = 50f;
    [SerializeField] float deactivateAngle = 0f;
    [SerializeField] float activateThreshold = 3f;
    [SerializeField] HingeJoint hinge;
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

    void Update()
    {
        if (!isDisabled)
        {
            // if (isActive)
            //{
            if (Mathf.Abs(hinge.angle - deactivateAngle) < activateThreshold)
            {
                Deactivate();
            }
            //}  
            // if (!isActive)
            // {
            if (Mathf.Abs(hinge.angle - activateAngle) < activateThreshold)
            {
                Activate();
            }
            // }

        }
    }
}