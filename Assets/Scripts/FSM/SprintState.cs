using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintState : PlayerMovementState
{
    public SprintState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        moveSpeed = 7f;
    }

    public override void Update()
    {
        if(sm.character.input.isSprinting == false)
        {
            sm.ChangeState(sm.walkState);
        }
        else
        {
            base.Update();
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
