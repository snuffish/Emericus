using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMovementBaseState
{
    
    public abstract void EnterState(PlayerMovementController player);
    
    public abstract void UpdateState(PlayerMovementController player);
    
    public abstract void ExitState(PlayerMovementController player);
}