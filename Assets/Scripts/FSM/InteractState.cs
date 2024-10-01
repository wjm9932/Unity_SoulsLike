using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractState : IState
{
    protected PlayerMovementStateMachine sm;

    public InteractState(PlayerMovementStateMachine sm)
    {
        this.sm = sm;
    }

    public virtual void Enter()
    {
    }
    public virtual void Update()
    {

    }
    public virtual void PhysicsUpdate()
    {

    }
    public virtual void LateUpdate()
    {
        UpdateAnimation();
    }
    public virtual void Exit()
    {

    }
    public virtual void OnAnimationEnterEvent()
    {

    }
    public virtual void OnAnimationExitEvent()
    {

    }
    public virtual void OnAnimationTransitionEvent()
    {

    }
    public virtual void OnAnimatorIK()
    {

    }

    void UpdateAnimation()
    {
        sm.owner.animator.SetFloat("Speed", sm.owner.rb.velocity.magnitude, 0.08f, Time.deltaTime);
    }
}
