using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLockOnState : CameraState
{
    public Transform target { get; private set; }
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
            csm.character.animator.SetBool("IsLockOn", true);
            csm.character.lockOnCamera.LookAt = target;
            csm.character.lockOffCamera.Priority = 9;
            csm.character.lockOnCamera.Priority = 10;

            UpdateCameraPosition();
            csm.character.StartCoroutine(ApplyDampingAfterFirstFrame());
        }
    }
    public override void Update()
    {
        base.Update();
        UpdateCameraPosition();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void LateUpdate()
    {
        base.LateUpdate();
    }
    public override void Exit()
    {
        base.Exit();
        csm.character.lockOnCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().Damping.x = 0f;
        target = null;
    }
    private void UpdateCameraPosition()
    {
        if (target != null)
        {
            Vector3 dir = (target.position - csm.character.camEyePos.position).normalized;
            Vector3 camPos = csm.character.camEyePos.position - dir * 4.5f;

            if (camPos.y < 0.2f)
            {
                camPos.y = 0.2f;
            }

            csm.character.lockOnCameraPosition.position = camPos;
            csm.character.lockOnCameraPosition.LookAt(target.position);
        }
    }

    private Transform ScanNearByEnemy()
    {
        Collider[] nearByTargets = Physics.OverlapSphere(csm.character.transform.position, 20f, csm.character.enemy);

        if (nearByTargets.Length <= 0)
        {
            return null;
        }
        else
        {
            Transform closetTarget = null;
            float closestAngle = 30f;
            Vector3 camForward = csm.character.mainCamera.transform.forward;
            camForward.y = 0f;

            for (int i = 0; i < nearByTargets.Length; i++)
            {
                Vector3 dir = nearByTargets[i].transform.position - csm.character.mainCamera.transform.position;
                dir.y = 0f;
                float angle = Vector3.Angle(camForward, dir);

                if (angle < closestAngle)
                {
                    closestAngle = angle;
                    closetTarget = nearByTargets[i].transform;
                }
            }
            return closetTarget;
        }
    }
    private IEnumerator ApplyDampingAfterFirstFrame()
    {
        yield return null;
        csm.character.lockOnCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().Damping.x = 1f;
    }
}
