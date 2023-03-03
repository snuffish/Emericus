using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recievers : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] bool resetStateOnDeactivation = true;

    [SerializeField, Tooltip("Disable If you want activation to keep happening, Example: A door trying to stay open as long as the lever is pulled")]
    bool stateHasToBeDifferent = true;

    [SerializeField, Tooltip("Does Interaction from the receiver to targets require all requirements to be met")]
    bool requireAllForActivation = true;

    [SerializeField, Tooltip("Activates All Targets on met requirements")]
    bool activateAllTargets = true;
    [SerializeField, Tooltip("Activate Targets On both true and false conditions")]
    bool activateOnAnyChange = false;

    [SerializeField, Tooltip("Deactivates All Targets On met requirements")]
    bool deactivateAllTargets = false;
    [SerializeField, Tooltip("Deactivate Targets On both true and false conditions")]
    bool deactivateOnAnyChange = false;

    [SerializeField, Tooltip("Invert Activation State of All Targets")]
    bool invertAllTargets = false;
    [SerializeField, Tooltip("Invert Targets On both true and false conditions")]
    bool invertOnChange = false;


    [Header("Activators")]
    [SerializeField]
    List<Activators> inputActivators = new List<Activators>();

    [SerializeField] List<Activators> targetActivators = new List<Activators>();
    Dictionary<Activators, bool> activators = new Dictionary<Activators, bool>();

    protected bool requirementsWereMet = false;

    void Start()
    {
        activators.Clear();
        requirementsWereMet = false;
        foreach (Activators activator in inputActivators)
        {
            activators.Add(activator, activator.IsActive);
            activator.OnChangeState.AddListener(ActivatorUpdate);
        }
    }

    public void ActivatorUpdate(Activators changedActivator, bool newState)
    {
        if (activators[changedActivator] != newState || !stateHasToBeDifferent)
        {
            activators[changedActivator] = newState;
            bool output = false;
            if (activateOnAnyChange)
            {
                foreach (Activators activator in targetActivators) activator.Activate();
            }
            else if (deactivateOnAnyChange)
            {
                foreach (Activators activator in targetActivators) activator.Deactivate();
            }
            else if (invertOnChange)
            {
                foreach (Activators activator in targetActivators) activator.InvertState();
            }
            if (requireAllForActivation)
            {
                if (newState == false)
                {
                    if (requirementsWereMet || !stateHasToBeDifferent)
                    {
                        requirementsWereMet = false;
                        if (invertAllTargets) foreach (Activators activator in targetActivators) activator.InvertState();
                        else foreach (Activators activator in targetActivators) activator.Deactivate();
                    }
                }
                else
                {
                    output = true;
                    foreach (Activators activator in inputActivators)
                    {
                        if (activators[activator] == false) output = false;
                    }
                }
                if (output)
                {
                    if (!requirementsWereMet || !stateHasToBeDifferent)
                    {
                        requirementsWereMet = true;
                        if (invertAllTargets) foreach (Activators activator in targetActivators) activator.InvertState();
                        else if (activateAllTargets) foreach (Activators activator in targetActivators) activator.Activate();
                        else if (deactivateAllTargets) foreach (Activators activator in targetActivators) activator.Deactivate();
                        else Debug.Log("Please pick how to interact with target on " + gameObject.name);
                    }
                }
                else if(resetStateOnDeactivation)
                {
                    if (requirementsWereMet || !stateHasToBeDifferent)
                    {
                        requirementsWereMet = false;
                        if (invertAllTargets) foreach (Activators activator in targetActivators) activator.InvertState();
                        else if (activateAllTargets) foreach (Activators activator in targetActivators) activator.Deactivate();
                        else if (deactivateAllTargets) foreach (Activators activator in targetActivators) activator.Activate();
                        else Debug.Log("Please pick how to interact with target on " + gameObject.name);
                    }
                }
            }
            else
            {
                output = false;
                foreach (Activators inputActivator in inputActivators)
                {
                    if (inputActivator.IsActive)
                    {
                        output = true;
                        if (invertAllTargets) foreach (Activators activator in targetActivators) activator.InvertState();
                        else if (activateAllTargets) foreach (Activators activator in targetActivators) activator.Activate();
                        else if (deactivateAllTargets) foreach (Activators activator in targetActivators) activator.Deactivate();
                        else Debug.Log("Please pick how to interact with target on " + gameObject.name);
                        break;
                    }
                }
                if (!output)
                {
                    if (resetStateOnDeactivation)
                    {
                        if (invertAllTargets) foreach (Activators activator in targetActivators) activator.InvertState();
                        else if (activateAllTargets) foreach (Activators activator in targetActivators) activator.Deactivate();
                        else if (deactivateAllTargets) foreach (Activators activator in targetActivators) activator.Activate();
                        
                    }
                }
            }

        }

    }
}