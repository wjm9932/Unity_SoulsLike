using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static System.TimeZoneInfo;

public class CameraLockOnState : CameraState
{
    public GameObject target { get; private set; }

    private Vector3 startPlayerCollisionPosition;
    private bool isCollisionDetected;
    private bool hasSwitched;
    private float rotationSpeed = 25f;
    private float downAngle = 7f;
    public CameraLockOnState(CameraStateMachine CameraStateMachine) : base(CameraStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        
        hasSwitched = false;
        isCollisionDetected = false;

        target = SetTarget();

        if (target == null)
        {
            csm.ChangeState(csm.cameraLockOffState);
        }
        else
        {
            csm.owner.animator.SetBool("IsLockOn", true);
            csm.owner.playerEvents.CameraLockOn();

            target.transform.root.GetComponent<Enemy>().lockOnIndicator.gameObject.SetActive(true);
            csm.owner.followCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().Damping.x = 0.8f;
        }
    }
    public override void Update()
    {
        if (target.transform.parent.GetComponent<LivingEntity>().isDead == true)
        {
            csm.ChangeState(csm.cameraLockOffState);
            return;
        }

        ChangeTarget(csm.owner.input.mouseInput.x);

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
        if (target != null)
        {
            target.transform.root.GetComponent<Enemy>().lockOnIndicator.gameObject.SetActive(false);
        }

        Vector3 currentEulerAngles = csm.owner.cameraTransform.eulerAngles;
        csm.owner.cameraTargetPitch = currentEulerAngles.x;
        csm.owner.cameraTargetYaw = currentEulerAngles.y;
    }
    private void UpdateCameraPosition()
    {
        Vector3 directionToTarget = (target.transform.position - csm.owner.camEyePos.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        Quaternion downRotation = Quaternion.Euler(downAngle, 0, 0);
        targetRotation = targetRotation * downRotation;

        csm.owner.cameraTransform.rotation = Quaternion.Slerp(csm.owner.cameraTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        if (Quaternion.Angle(csm.owner.cameraTransform.rotation, targetRotation) < 1f)
        {
            csm.owner.cameraTransform.rotation = targetRotation;
        }
    }

    private GameObject SetTarget()
    {
        Collider[] nearByTargets = ScanNearByEnemy();

        if (nearByTargets == null)
        {
            return null;
        }
        else
        {
            GameObject closestTarget = null;
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
                        closestTarget = nearByTargets[i].gameObject;
                    }
                }
            }
            return closestTarget;
        }
    }

    void ChangeTarget(float mouseDir)
    {
        if(Mathf.Abs(mouseDir) > 2f && hasSwitched == false)
        {
            hasSwitched = true;
            
            Collider[] nearByTargets = ScanNearByEnemy();
            if (nearByTargets == null)
            {
                return;
            }

            GameObject closestTarget = null;
            Vector3 camForward = csm.owner.mainCamera.transform.forward;
            camForward.y = 0f;
            float closestAngle = 50f;

            for (int i = 0; i < nearByTargets.Length; i++)
            {
                if(target == nearByTargets[i].gameObject)
                {
                    continue;
                }

                Vector3 dir = (nearByTargets[i].transform.position - csm.owner.camEyePos.position).normalized;

                if (Physics.Raycast(csm.owner.camEyePos.position, dir, dir.magnitude, csm.owner.lockOnCameraTargetLayer) == true)
                {
                    continue;
                }
                else
                {
                    float value = Vector3.Cross(camForward, dir).y;

                    if (Mathf.Sign(mouseDir) != Mathf.Sign(value))
                    {
                        continue;
                    }

                    dir.y = 0f;
                    float angle = Vector3.Angle(camForward, dir);
                    if (angle < closestAngle)
                    {
                        closestAngle = angle;
                        closestTarget = nearByTargets[i].gameObject;
                    }
                }
            }

            if (closestTarget != null)
            {
                target.transform.root.GetComponent<Enemy>().lockOnIndicator.gameObject.SetActive(false);
                closestTarget.transform.root.GetComponent<Enemy>().lockOnIndicator.gameObject.SetActive(true);
                target = closestTarget;
            }
        }
        else if(Mathf.Abs(mouseDir) <= 1f)
        {
            hasSwitched = false;
        }
    }

    private Collider[] ScanNearByEnemy()
    {
        Collider[] nearByTargets = Physics.OverlapSphere(csm.owner.transform.position, 20f, csm.owner.enemyMask);

        if (nearByTargets.Length <= 0)
        {
            return null;
        }

        return nearByTargets;
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
            if (Vector3.Distance(csm.owner.transform.position, startPlayerCollisionPosition) > 0.5f)
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
