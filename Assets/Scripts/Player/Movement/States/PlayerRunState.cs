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
    }
}
