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
        base.Enter();
        moveSpeed = 5f;
    }

    public override void Update()
    {
        if(sm.character.input.isSprinting == true)
        {
            sm.ChangeState(sm.sprintState);
        }
        else if(CameraStateMachine.Instance.currentState == CameraStateMachine.Instance.cameraLockOnState)
        {
            sm.ChangeState(sm.lockOnWalkState);
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
