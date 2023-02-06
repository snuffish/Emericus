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
    }
    
    
}
