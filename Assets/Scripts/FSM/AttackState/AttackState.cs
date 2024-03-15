using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackState : IState
{
    protected PlayerMovementStateMachine sm;
    protected float dashForce;
    protected bool canComboAttack;
    Quaternion rotation;
    public AttackState(PlayerMovementStateMachine sm)
    {
        this.sm = sm;
    }

    public virtual void Enter()
    {
        sm.character.rb.velocity = Vector3.zero;

        Vector3 forward = sm.character.followCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 dir = sm.character.transform.position + forward;
        rotation = Quaternion.LookRotation(forward);

        sm.character.rb.AddForce(forward * dashForce, ForceMode.Force);
    }
    public virtual void Update()
    {
        if (sm.character.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && sm.character.animator.IsInTransition(0) == false)
        {
            sm.character.animator.SetTrigger("ResetAttackCombo");
            sm.ChangeState(sm.idleState);
        }
        else
        {
            sm.character.transform.rotation = Quaternion.Slerp(sm.character.transform.rotation, rotation, 10f * Time.deltaTime);
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
