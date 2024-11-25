using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttackState : IState
{
    protected PlayerMovementStateMachine sm;

    private bool isDone;
    public SlashAttackState(PlayerMovementStateMachine sm)
    {
        this.sm = sm;
    }
    public virtual void Enter()
    {
        isDone = false;
        sm.owner.canBeDamaged = false;
        sm.owner.animator.SetBool("IsSlash", true);
    }
    public virtual void Update()
    {
        if (isDone == true)
        {
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
        sm.owner.animator.SetBool("IsSlash", false);
    }
    public virtual void OnAnimationEnterEvent()
    {

    }
    public virtual void OnAnimationExitEvent()
    {
        isDone = true;
    }
    public virtual void OnAnimationTransitionEvent()
    {
        var slash = Object.Instantiate(sm.owner.slash, sm.owner.GetPlayerPosition(), sm.owner.transform.rotation).GetComponent<Slash>();
        slash.SetOwner(sm.owner.gameObject);
        SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.SLASH, 0.5f);
        sm.owner.canBeDamaged = true;
    }
    public virtual void OnAnimatorIK()
    {
        sm.owner.animator.SetFloat("HandWeight", 0);
    }

}