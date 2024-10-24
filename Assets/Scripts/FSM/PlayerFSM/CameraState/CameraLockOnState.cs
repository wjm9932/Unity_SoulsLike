using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLockOnState : CameraState
{
    public GameObject target { get; private set; }

    Vector3 startPlayerCollisionPosition;
    bool isCollisionDetected;
    public CameraLockOnState(CameraStateMachine CameraStateMachine) : base(CameraStateMachine)
    {
    }
    public override void Enter()
    {
        isCollisionDetected = false;
        base.Enter();

        target = ScanNearByEnemy();

        if (target == null)
        {
            csm.ChangeState(csm.cameraLockOffState);
        }
        else
        {
            csm.owner.animator.SetBool("IsLockOn", true);
            csm.owner.lockOnCamera.LookAt = target.transform;
            csm.owner.lockOffCamera.Priority = 9;
            csm.owner.lockOnCamera.Priority = 10;
            csm.owner.playerEvents.CameraLockOn();

            target.transform.root.GetComponent<Enemy>().lockOnIndicator.gameObject.SetActive(true);
            csm.owner.StartCoroutine(ApplyDampingAfterTwoFrame());
        }
    }
    public override void Update()
    {
        if (target.transform.parent.GetComponent<LivingEntity>().isDead == true)
        {
            csm.ChangeState(csm.cameraLockOffState);
            return;
        }

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
        if(target != null)
        {
            target.transform.root.GetComponent<Enemy>().lockOnIndicator.gameObject.SetActive(false);
        }
        csm.owner.lockOnCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().Damping.x = 0f;
    }
    private void UpdateCameraPosition()
    {
        Vector3 dir = (target.transform.position - csm.owner.camEyePos.position).normalized;
        Vector3 camPos = csm.owner.camEyePos.position - dir * 4.5f;

        if (camPos.y < 0.2f)
        {
            camPos.y = 0.2f;
        }

        csm.owner.lockOnCameraPosition.position = camPos;
        csm.owner.lockOnCameraPosition.LookAt(target.transform.position);
    }

    private GameObject ScanNearByEnemy()
    {
        Collider[] nearByTargets = Physics.OverlapSphere(csm.owner.transform.position, 20f, csm.owner.enemyMask);

        if (nearByTargets.Length <= 0)
        {
            return null;
        }
        else
        {
            GameObject closetTarget = null;
            float closestAngle = 30f;
            Vector3 camForward = csm.owner.mainCamera.transform.forward;
            camForward.y = 0f;

            for (int i = 0; i < nearByTargets.Length; i++)
            {
                Vector3 dir = nearByTargets[i].transform.position - csm.owner.camEyePos.position;

                if (Physics.Raycast(csm.owner.camEyePos.position, dir, dir.magnitude, csm.owner.lockOnCameraTargetLayer) == true)
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
                        closetTarget = nearByTargets[i].gameObject;
                    }
                }
            }
            return closetTarget;
        }
    }
    private IEnumerator ApplyDampingAfterTwoFrame()
    {
        yield return null;
        yield return null;
        csm.owner.lockOnCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().Damping.x = 1f;
    }

    
    private void CheckCameraCollision()
    {
        Vector3 cameraPosition = csm.owner.camEyePos.position;
        Vector3 direction = (target.transform.position - cameraPosition).normalized;
        float distanceToTarget = Vector3.Distance(cameraPosition, target.transform.position);

        if (Physics.Raycast(cameraPosition, direction, distanceToTarget, csm.owner.lockOnCameraTargetLayer) == true)
        {
            if (!isCollisionDetected)
            {
                isCollisionDetected = true;
                startPlayerCollisionPosition = csm.owner.transform.position;
            }
            if (Vector3.Distance(csm.owner.transform.position, startPlayerCollisionPosition) > 1f)
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
