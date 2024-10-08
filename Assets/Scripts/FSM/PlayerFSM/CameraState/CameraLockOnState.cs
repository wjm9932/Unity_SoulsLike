using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLockOnState : CameraState
{
    public Transform target { get; private set; }

    Vector3 startPlayerCollisionPosition;
    bool isCollisionDetected;
    public CameraLockOnState(CameraStateMachine CameraStateMachine) : base(CameraStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();

        target = ScanNearByEnemy();

        if (target == null)
        {
            csm.ChangeState(csm.cameraLockOffState);
        }
        else
        {
            csm.owner.animator.SetBool("IsLockOn", true);
            csm.owner.lockOnCamera.LookAt = target;
            csm.owner.lockOffCamera.Priority = 9;
            csm.owner.lockOnCamera.Priority = 10;

            UpdateCameraPosition();
            csm.owner.StartCoroutine(ApplyDampingAfterFirstFrame());
        }
    }
    public override void Update()
    {
        UpdateCameraPosition();
        base.Update();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        CheckCameraCollision();
    }
    public override void LateUpdate()
    {
        base.LateUpdate();
    }
    public override void Exit()
    {
        base.Exit();
        csm.owner.lockOnCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().Damping.x = 0f;
    }
    private void UpdateCameraPosition()
    {
        Vector3 dir = (target.position - csm.owner.camEyePos.position).normalized;
        Vector3 camPos = csm.owner.camEyePos.position - dir * 4.5f;

        if (camPos.y < 0.2f)
        {
            camPos.y = 0.2f;
        }

        csm.owner.lockOnCameraPosition.position = camPos;
        csm.owner.lockOnCameraPosition.LookAt(target.position);
    }

    private Transform ScanNearByEnemy()
    {
        Collider[] nearByTargets = Physics.OverlapSphere(csm.owner.transform.position, 20f, csm.owner.enemyMask);

        if (nearByTargets.Length <= 0)
        {
            return null;
        }
        else
        {
            Transform closetTarget = null;
            float closestAngle = 30f;
            Vector3 camForward = csm.owner.mainCamera.transform.forward;
            camForward.y = 0f;

            for (int i = 0; i < nearByTargets.Length; i++)
            {
                Vector3 dir = nearByTargets[i].transform.position - csm.owner.transform.position;

                if (Physics.Raycast(csm.owner.transform.position, dir, dir.magnitude, csm.owner.lockOnCameraTargetLayer) == true)
                {
                    continue;
                }
                else
                {
                    dir.y = 0f;
                    float angle = Vector3.Angle(camForward, dir);
                    if (angle < closestAngle)
                    {
                        closestAngle = angle;
                        closetTarget = nearByTargets[i].transform;
                    }
                }
            }
            return closetTarget;
        }
    }
    private IEnumerator ApplyDampingAfterFirstFrame()
    {
        yield return null;
        csm.owner.lockOnCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().Damping.x = 1f;
    }

    
    private void CheckCameraCollision()
    {
        Vector3 cameraPosition = csm.owner.camEyePos.position;
        Vector3 direction = (target.position - cameraPosition).normalized;
        float distanceToTarget = Vector3.Distance(cameraPosition, target.position);

        if (Physics.Raycast(cameraPosition, direction, distanceToTarget, csm.owner.lockOnCameraTargetLayer) == true)
        {
            if (!isCollisionDetected)
            {
                isCollisionDetected = true;
                startPlayerCollisionPosition = csm.owner.transform.position;
            }
            if (Vector3.Distance(csm.owner.transform.position, startPlayerCollisionPosition) > 1.5f)
            {
                csm.ChangeState(csm.cameraLockOffState);
            }
        }
        else
        {
            isCollisionDetected = false;
            startPlayerCollisionPosition = csm.owner.transform.position;
        }
    }
}
