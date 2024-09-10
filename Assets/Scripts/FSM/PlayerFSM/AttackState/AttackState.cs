using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackState : IState
{
    protected PlayerMovementStateMachine sm;
    protected float dashForce;
    protected bool canComboAttack;
    protected bool canAttack;
    Quaternion rotation;

    public AttackState(PlayerMovementStateMachine sm)
    {
        this.sm = sm;
        canAttack = false;
    }
    public virtual void Enter()
    {
        AddAttackDashForce();
        sm.owner.swordEffect.enabled = true;
    }
    public virtual void Update()
    {
        if (sm.owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && sm.owner.animator.IsInTransition(0) == false)
        {
            sm.owner.animator.SetTrigger("ResetAttackCombo");
            sm.ChangeState(sm.idleState);
        }
        else
        {
            sm.owner.transform.rotation = Quaternion.Slerp(sm.owner.transform.rotation, rotation, 20f * Time.deltaTime);
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
        sm.owner.swordEffect.enabled = false;
        sm.owner.rb.drag = 0f;
        sm.owner.attack.SetCanAttack(0);
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
    public virtual void OnAnimatorIK()
    {
        sm.owner.animator.SetFloat("HandWeight", 0);
    }
    private void AddAttackDashForce()
    {
        sm.owner.rb.velocity = Vector3.zero;
        sm.owner.rb.drag = 2f;

        Vector3 forward = sm.owner.mainCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();

        rotation = Quaternion.LookRotation(forward);

        sm.owner.rb.AddForce(forward * dashForce, ForceMode.Impulse);
    }
}
