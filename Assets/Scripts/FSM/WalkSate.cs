using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class WalkSate : PlayerMovementState
{
    public WalkSate(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {
        moveSpeed = 5f;
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (sm.character.input.moveInput == Vector2.zero)
        {
            sm.ChangeState(sm.idleState);
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
