using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerMovementBaseState
{
    public override void EnterState(PlayerMovementController player) {
        //player.animator.SetTrigger("isRunning");
        player.currentMoveSpeed = player.runSpeed;
    }
    
    public override void UpdateState(PlayerMovementController player) {
        player.GetInput();
        player.LimitSpeed();
        player.MovePlayer();
        
        if(Input.GetButtonUp("Sprint"))
            player.ChangeState(player.walkState);
        
        if(Input.GetButtonDown("Crouch"))
            player.ChangeState(player.crouchState);
        
        if(player.deltaMovement.magnitude < 0.01)
            player.ChangeState(player.idleState);
    }
}
