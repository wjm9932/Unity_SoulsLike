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
        sm.character.rb.velocity = Vector3.zero;
    }
    public override void Update()
    {
        //base.Update();
        if(sm.character.input.moveInput != Vector2.zero)
        { 
            if(CameraStateMachine.Instance.currentState == CameraStateMachine.Instance.cameraLockOffState)
            {
                sm.ChangeState(sm.walkState);
            }
            else
            {
                sm.ChangeState(sm.lockOnWalkState);
            }
        }
        if(sm.character.input.isDodging == true)
        {
            sm.ChangeState(sm.dodgeState);
        }
        if (sm.character.input.isAttack == true)
        {
            sm.ChangeState(sm.combo_1AttackState);
        }
    }
    public override void PhysicsUpdate()
    {
        //base.PhysicsUpdate();
    }
    public override void LateUpdate()
    {
        //base.LateUpdate();
        UpdateAnimation();
    }
    public override void Exit()
    {
        //base.Exit();
    }
    void UpdateAnimation()
    {
        sm.character.animator.SetFloat("Speed", sm.character.rb.velocity.magnitude, 0.08f, Time.deltaTime);
    }
}
