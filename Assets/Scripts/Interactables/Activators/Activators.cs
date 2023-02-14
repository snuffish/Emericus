using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Activators : Interactable
{
    //public delegate void Activator(Activators activator, bool isActive);

    public UnityEvent<Activators, bool> OnChangeState = new UnityEvent<Activators, bool>();
    [SerializeField] protected bool isActive;

    public bool IsActive
    {
        get { return isActive; }

        set { ChangeState(value); }
    }
    void Start()
    {
        ChangeState(isActive);
    }


    public override void Interact()
    {
        if (!isLocked)
        {
            InvertState();
        }
    }

    public virtual void ChangeState(bool toState)
    {
        isActive = toState;
        OnChangeState.Invoke(this, isActive);
    }

    public virtual void Activate()
    {
        ChangeState(true);
    }

    public virtual void Deactivate()
    {
        ChangeState(false);
    }

    public virtual void InvertState()
    {
        ChangeState(!isActive);
    }

}