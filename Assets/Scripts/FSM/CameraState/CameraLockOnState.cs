using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLockOnState : CameraState
{
    private Vector3 target;
    public CameraLockOnState(CameraStateMachine CameraStateMachine) : base(CameraStateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter(); 

        target = new Vector3(0.18f, 1.57f, 10.11f);
        Vector3 targetDirection = target - csm.character.transform.position;
        targetDirection.y = 0;
        csm.character.transform.rotation = Quaternion.LookRotation(targetDirection);

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
