using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo_2AttackState : AttackState
{
    public Combo_2AttackState(PlayerMovementStateMachine sm) : base(sm)
    {
    }
    public override void Enter()
    {
        dashForce = 100f;
        base.Enter();
        sm.character.animator.SetTrigger("IsAttack2");
        canComboAttack = false;
    }

    public override void Update()
    {
        if (canComboAttack == true && sm.character.input.isAttack == true)
        {
            sm.ChangeState(sm.combo_3AttackState);
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

    public override void OnAnimationEnterEvent()
    {
        base.OnAnimationEnterEvent();
    }
    public override void OnAnimationExitEvent()
    {
        base.OnAnimationExitEvent();
    }
    public override void OnAnimationTransitionEvent()
    {
        base.OnAnimationTransitionEvent();
    }
}
