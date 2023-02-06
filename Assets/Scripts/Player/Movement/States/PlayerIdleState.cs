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
    }
}
