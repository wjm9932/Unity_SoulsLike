using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo_3AttackState : AttackState
{
    public Combo_3AttackState(PlayerMovementStateMachine sm) : base(sm)
    {
    }
    public override void Enter()
    {
        dashForce = 250f;
        base.Enter();

        sm.character.animator.SetTrigger("IsAttack3");
        canComboAttack = false;
    }

    public override void Update()
    {
        base.Update();
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
