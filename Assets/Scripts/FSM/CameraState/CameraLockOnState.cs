using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLockOnState : CameraState
{
    public CameraLockOnState(CameraStateMachine CameraStateMachine) : base(CameraStateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();

        csm.character.animator.SetBool("IsLockOn", true);

        csm.character.lockOffCamera.Priority = 9;
        csm.character.lockOnCamera.Priority = 10;
    }
    public override void Update()
    {
        base .Update();
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
