using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : IState
{
    protected PlayerMovementStateMachine sm;

    public InteractState(PlayerMovementStateMachine sm)
    {
        this.sm = sm;
    }

    public void Enter()
    {
    }
    public void Update()
    {
        sm.owner.rb.velocity = Vector3.zero;

        if(sm.owner.uiStateMachine.currentState is InteractStateUI == false)
        {
            sm.ChangeState(sm.idleState);
        }
    }
    public void PhysicsUpdate()
    {

    }
    public void LateUpdate()
    {
        UpdateAnimation();
    }
    public void Exit()
    {

    }
    public void OnAnimationEnterEvent()
    {

    }
    public void OnAnimationExitEvent()
    {

    }
    public void OnAnimationTransitionEvent()
    {

    }
    public void OnAnimatorIK()
    {
        sm.owner.animator.SetFloat("HandWeight", 1, 0.1f, Time.deltaTime * 0.1f);
        sm.owner.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, sm.owner.animator.GetFloat("HandWeight"));
        sm.owner.animator.SetIKPosition(AvatarIKGoal.LeftHand, sm.owner.leftHandPos.position);
    }

    void UpdateAnimation()
    {
        sm.owner.animator.SetFloat("Speed", sm.owner.rb.velocity.magnitude, 0.08f, Time.deltaTime);
    }
}
