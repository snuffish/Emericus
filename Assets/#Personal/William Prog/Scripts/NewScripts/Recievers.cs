using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recievers : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Tooltip("Does Interaction from the receiver to targets require all requirements to be met")]
    bool allTargetsAreRequiredForActivation = true;

    [SerializeField, Tooltip("Activates All Targets on met requirements")]
    bool activateAllTargets = true;
    bool activateAllTargetsOnUnmetRequirements = false;

    [SerializeField, Tooltip("Deactivates All Targets On met requirements")]
    bool deactivateAllTargets = false;
    bool deactivateAllTargetsOnUnmetRequirements = false;

    [SerializeField, Tooltip("Invert Activation State of All Targets")]
    bool invertAllTargets = false;

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
        if (activators[changedActivator] != newState)
        {
            activators[changedActivator] = newState;
            bool output = false;
            if (allTargetsAreRequiredForActivation)
            {
                if (newState == false)
                {
                    if (requirementsWereMet)
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
                    if (!requirementsWereMet)
                    {
                        requirementsWereMet = true;
                        if (invertAllTargets) foreach (Activators activator in targetActivators) activator.InvertState();
                        else if (activateAllTargets) foreach (Activators activator in targetActivators) activator.Activate();
                        else if (deactivateAllTargets) foreach (Activators activator in targetActivators) activator.Deactivate();
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
                    }
                }
                if (!output)
                {
                    if (invertAllTargets) foreach (Activators activator in targetActivators) activator.InvertState();
                    else foreach (Activators activator in targetActivators) activator.Deactivate();
                }
            }

        }

    }
}