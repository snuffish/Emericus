using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerMovementBaseState
{
    public override void EnterState(PlayerMovementController player) {
        //player.animator.SetTrigger("isCrouching");
        player.currentMoveSpeed = player.crouchSpeed;
        player.transform.localScale = new Vector3(player.transform.localScale.x, player.transform.localScale.y * 0.5f, player.transform.localScale.z);
        
        if(player.isGrounded)
            player.transform.position -= Vector3.up * 0.85f;
        else
            player.transform.position += Vector3.up * 0.5f;
        
    }
    
    public override void UpdateState(PlayerMovementController player) {
        player.GetInput();
        player.LimitSpeed();
        player.MovePlayer();
        
        if(Input.GetButtonUp("Crouch"))
            player.ChangeState(player.idleState);
        
        if (player.currentTime >= player.stepInterval) { 
            player.currentTime = 0;
            player.playerAudio.PlayFootstep(player.gameObject);
        }                       
        else {
            player.currentTime += Time.deltaTime;
        }
    }
    
    public override void ExitState(PlayerMovementController player) {
        if(player.isGrounded)
            player.transform.position += Vector3.up * 0.5f;
        else
            player.transform.position -= Vector3.up * 0.5f;
        
        player.transform.localScale = new Vector3(player.transform.localScale.x, player.transform.localScale.y * 2f, player.transform.localScale.z);
    }
}
