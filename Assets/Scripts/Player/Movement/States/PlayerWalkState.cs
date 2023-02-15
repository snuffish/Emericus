using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerMovementBaseState
{
    public override void EnterState(PlayerMovementController player) {
        //player.animator.SetTrigger("isWalking");
        player.currentMoveSpeed = player.walkSpeed;
    }
    
    public override void UpdateState(PlayerMovementController player) {
        
        player.GetInput();
        player.LimitSpeed();
        player.MovePlayer();
        
        if(Input.GetButtonDown("Sprint"))
            player.ChangeState(player.runState);
        
        if(Input.GetButtonDown("Crouch"))
            player.ChangeState(player.crouchState);
        
        if(player.deltaMovement.magnitude < 0.01)
            player.ChangeState(player.idleState);
        
        
        
        if (player.currentTime >= player.stepInterval)
        {
            //Only play Footsteps while grounded (Leon) Clueless
            if (player.isGrounded)
            {
                player.playerAudio.PlayFootstep(player.gameObject);
            }
            player.currentTime = 0;
        }
        else player.currentTime += Time.deltaTime;
        
        
        if (player.currentTime >= player.stepInterval) { 
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
