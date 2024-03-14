using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    protected PlayerMovementStateMachine sm;
    protected bool canComboAttack;
    public AttackState(PlayerMovementStateMachine sm)
    {
        this.sm = sm;
    }

    public virtual void Enter()
    {
        sm.character.rb.velocity = Vector3.zero;
        sm.character.rb.AddForce(sm.character.transform.forward * 150f, ForceMode.Force);
    }
    public virtual void Update()
    {
        if (sm.character.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && sm.character.animator.IsInTransition(0) == false)
        {
            sm.character.animator.SetTrigger("ResetAttackCombo");
            sm.ChangeState(sm.idleState);
        }
    }
    public virtual void PhysicsUpdate()
    {

    }
    public virtual void LateUpdate()
    {

    }
    public virtual void Exit()
    {
        canComboAttack = true;
    }
    public virtual void OnAnimationEnterEvent()
    {

    }
    public virtual void OnAnimationExitEvent()
    {

    }
    public virtual void OnAnimationTransitionEvent()
    {
        canComboAttack = true;
    }
}
