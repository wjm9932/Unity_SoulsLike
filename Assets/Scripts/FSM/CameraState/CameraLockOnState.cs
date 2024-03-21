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
        target = ScanNearByEnemy();

        if(target == null)
        {
            csm.ChangeState(csm.cameraLockOffState);
        }
        else
        {
            csm.character.animator.SetBool("IsLockOn", true);
            csm.character.lockOffCamera.Priority = 9;
            csm.character.lockOnCamera.Priority = 10;
        }
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
    private Transform ScanNearByEnemy()
    {
        Collider[] nearByTargets = Physics.OverlapSphere(csm.character.transform.position, 20f, csm.character.enemy);
        if(nearByTargets.Length <= 0)
        {
            return null;
        }
        else
        {
            Transform closetTarget = null;
            float closestAngle = 90f;
            Vector3 camForward = csm.character.followCamera.transform.forward;
            camForward.y = 0f;

            for (int i = 0; i < nearByTargets.Length; i++)
            {
                Vector3 dir = nearByTargets[i].transform.position - csm.character.followCamera.transform.position;
                dir.y = 0f;
                float angle = Vector3.Angle(camForward, dir);

                if(angle < closestAngle)
                {
                    closestAngle = angle;
                    closetTarget = nearByTargets[i].transform;
                }
            }

            return closetTarget;
        }

    }
}
