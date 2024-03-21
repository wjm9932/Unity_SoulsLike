using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraState : IState
{
    protected CameraStateMachine csm;
    protected Transform target;

    private Vector3 playerHeadPos;
   public CameraState(CameraStateMachine csm)
    {
        this.csm = csm;
    }
    public virtual void Enter()
    {
        
    }
    public virtual void Update()
    {
        UpdateCameraPosition();

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
    private void UpdateCameraPosition()
    {
        playerHeadPos = csm.character.transform.position;
        playerHeadPos.y = csm.character.playerHeight;

        Vector3 dir = (csm.character.tempTarget.transform.position - playerHeadPos).normalized;
        Vector3 camPos = playerHeadPos - dir * 5f;

        if (camPos.y < 0.2f)
        {
            camPos.y = 0.2f;
        }

        //csm.character.lockOnCameraPosition.position = csm.character.transform.position;
        csm.character.lockOnCameraPosition.position = camPos;
        csm.character.lockOnCameraPosition.LookAt(csm.character.tempTarget.transform.position);
    }
}
