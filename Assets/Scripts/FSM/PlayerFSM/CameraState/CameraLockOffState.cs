using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockOffState : CameraState
{
    public CameraLockOffState(CameraStateMachine CameraStateMachine) : base(CameraStateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();
        csm.owner.lockOnCamera.LookAt = null;
        csm.owner.animator.SetBool("IsLockOn", false);

        csm.owner.lockOffCamera.Priority = 10;
        csm.owner.lockOnCamera.Priority = 9;
    }
    public override void Update()
    {
        base.Update();
    }
    public override void PhysicsUpdate()
    {
        //base.PhysicsUpdate();
    }
    public override void LateUpdate()
    {
        //base.LateUpdate();
    }
    public override void Exit()
    {
        //base.Exit();
    }
}
