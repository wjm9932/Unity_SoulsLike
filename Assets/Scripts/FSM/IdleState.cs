using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerMovementState
{
    public IdleState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {
        //base.Enter();
    }

    public override void Update()
    {
        //base.Update();
        if(sm.character.input.moveInput != Vector2.zero)
        {
            sm.ChangeState(sm.walkState);
        }
    }

    public override void PhysicsUpdate()
    {
        //base.PhysicsUpdate();
    }

    public override void LateUpdate()
    {
        //base.LateUpdate();
    }

    public override void Exit()
    {
        //base.Exit();
    }

}
