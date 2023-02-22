using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerMovementBaseState
{
    public override void EnterState(PlayerMovementController player) {
        //player.animator.SetTrigger("isRunning");
        player.currentMoveSpeed = player.runSpeed;
        player.currentStepInterval = player.stepIntervalRun;
    }
    
    public override void UpdateState(PlayerMovementController player) {
        player.GetInput();
        player.LimitSpeed();
        player.MovePlayer();
        
        if(Input.GetButtonUp("Sprint"))
            player.ChangeState(player.walkState);
        
        if(Input.GetButtonDown("Crouch"))
            player.ChangeState(player.crouchState);
        
        if(player.deltaMovement.magnitude < 0.1)
            player.ChangeState(player.idleState);
        
        //  Footsteps
        if (player.currentTime >= player.currentStepInterval & player.isGrounded) { 
            player.currentTime = 0;
            player.playerAudio.PlayFootstep(player.gameObject);
        }                       
        else {
            player.currentTime += Time.deltaTime;
        }
    }
    
    public override void ExitState(PlayerMovementController player) {
        
    }
}
