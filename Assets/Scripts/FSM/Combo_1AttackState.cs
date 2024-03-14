using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Combo_1AttackState : AttackState
{
    
    public Combo_1AttackState(PlayerMovementStateMachine sm) : base(sm)
    {
    }

    public override void Enter()
    {
        dashForce = 150f;
        base.Enter();
        sm.character.animator.SetTrigger("IsAttack1");
        canComboAttack = false;
    }

    public override void Update()
    {
        if(canComboAttack == true && sm.character.input.isAttack == true)
        {
            sm.ChangeState(sm.combo_2AttackState);
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
