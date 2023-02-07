using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerMovementBaseState
{
    public override void EnterState(PlayerMovementController player) {
        //player.animator.SetTrigger("isIdle");
    }
    
    public override void UpdateState(PlayerMovementController player) {
        player.GetInput();
        
        if(player.deltaMovement.magnitude > 0)
            player.ChangeState(player.walkState);
        
        if(Input.GetButtonDown("Sprint"))
            player.ChangeState(player.runState);
        
        if(Input.GetButtonDown("Crouch"))
            player.ChangeState(player.crouchState);
        
        
    }
    
    public override void ExitState(PlayerMovementController player) {
        
    }
}
