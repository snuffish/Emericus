using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerMovementBaseState
{
    public override void EnterState(PlayerMovementController player) {
        //player.animator.SetTrigger("isCrouching");
        player.currentMoveSpeed = player.crouchSpeed;
    }
    
    public override void UpdateState(PlayerMovementController player) {
        player.GetInput();
    }
}
