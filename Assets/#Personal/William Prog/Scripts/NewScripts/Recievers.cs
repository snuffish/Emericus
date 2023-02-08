using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recievers : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Tooltip("Does Interaction from the receiver to targets require all requirements to be met")]
    bool allTargetsAreRequiredForActivation = true;

    [SerializeField, Tooltip("Activates All Targets")]
    bool activateAllTargets = true;

    [SerializeField, Tooltip("Deactivates All Targets")]
    bool deactivateAllTargets = false;

    [SerializeField, Tooltip("Invert Activation State of All Targets")]
    bool invertAllTargets = false;

    [Header("Activators")] [SerializeField]
    List<Activators> inputActivators = new List<Activators>();

    [SerializeField] List<Activators> targetActivators = new List<Activators>();
    Dictionary<Activators, bool> activators = new Dictionary<Activators, bool>();

    void Start()
    {
        activators.Clear();
        foreach (Activators activator in inputActivators)
        {
            activators.Add(activator, activator.IsActive);
            activator.OnChangeState.AddListener(ActivatorUpdate);
        }
    }

   public void ActivatorUpdate(Activators changedActivator, bool newState)
    {
        bool output;
        if (allTargetsAreRequiredForActivation)
        {
            output = true;
        }
        else output = false;

        activators[changedActivator] = newState;
        foreach (Activators inputActivator in inputActivators)
        {
            if (!inputActivator.IsActive && allTargetsAreRequiredForActivation) output = false;
            else if (inputActivator.IsActive && !allTargetsAreRequiredForActivation) output = true;
        }

        if (output)
        {
            print(output);
            foreach (Activators activator in targetActivators)
            {
                if (activateAllTargets) activator.Activate();
                else if (deactivateAllTargets) activator.Deactivate();
                else if (invertAllTargets) activator.InvertState();
            }
        }
    }
}