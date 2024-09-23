using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraState : IState
{
    protected CameraStateMachine csm;
   public CameraState(CameraStateMachine csm)
    {
        this.csm = csm;
    }
    public virtual void Enter()
    {
        
    }
    public virtual void Update()
    {
        if (csm.owner.input.isLockOn == true)
        {
            if(csm.currentState == csm.cameraLockOnState)
            {
                csm.ChangeState(csm.cameraLockOffState);
            }
            else
            {
                csm.ChangeState(csm.cameraLockOnState);
            }
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
}
