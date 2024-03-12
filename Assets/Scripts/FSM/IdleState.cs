using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerMovementState
{
    private PlayerMovementStateMachine playerMovementStateMachine;
    public IdleState(PlayerMovementStateMachine playerMovementStateMachine)
    {
        this.playerMovementStateMachine = playerMovementStateMachine;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if(playerMovementStateMachine.character.input.moveInput != Vector2.zero)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.walkState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }

    public override void Exit()
    {
        base.Exit();
    }
}
