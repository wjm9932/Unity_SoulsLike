using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraState : IState
{
    protected CameraStateMachine csm;
    protected Vector3 target;
   public CameraState(CameraStateMachine csm)
    {
        this.csm = csm;
    }
    public virtual void Enter()
    {
        
    }
    public virtual void Update()
    {
        csm.character.lockOnCameraPosition.position = csm.character.transform.position;
        csm.character.lockOnCameraPosition.LookAt(new Vector3(0.18f, 1.57f, 10.11f));

        if (csm.character.input.isLockOn == true)
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
}
