using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CameraLockOffState : CameraState
{
    float cameraTargetYaw;
    float cameraTargetPitch;
    float bottonClamp;
    float topClamp;

    private IEnumerator coroutineReference;
    public CameraLockOffState(CameraStateMachine CameraStateMachine) : base(CameraStateMachine)
    {
        bottonClamp = -30f;
        topClamp = 35f;
    }
    public override void Enter()
    {
        base.Enter();
        csm.owner.animator.SetBool("IsLockOn", false);
        cameraTargetYaw = csm.owner.cameraTargetYaw;
        cameraTargetPitch = csm.owner.cameraTargetPitch;

        coroutineReference = ReduceDamping();
        csm.owner.StartCoroutine(coroutineReference);
    }
    public override void Update()
    {
        base.Update();
        CameraRotation();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void LateUpdate()
    {
        //base.LateUpdate();
    }
    public override void Exit()
    {
        if(coroutineReference != null)
        {
            csm.owner.StopCoroutine(coroutineReference);
        }
        //base.Exit();
    }

    private void CameraRotation()
    {
        if(csm.owner.input.canGetMouseInput == true)
        {
            cameraTargetYaw += csm.owner.input.mouseInput.x * 1.8f;
            cameraTargetPitch -= csm.owner.input.mouseInput.y;

            cameraTargetPitch = ClampAngle(cameraTargetPitch, bottonClamp, topClamp);

            csm.owner.cameraTransform.rotation = Quaternion.Euler(cameraTargetPitch, cameraTargetYaw, 0.0f);
        }
    }

    private float ClampAngle(float angle, float minClamp, float maxClamp)
    {
        angle %= 360;
        if (angle > 180) angle -= 360;
        else if (angle < -180) angle += 360;

        return Mathf.Clamp(angle, minClamp, maxClamp);
    }

    private IEnumerator ReduceDamping()
    {
        var cinemachineComponent = csm.owner.followCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        float dampingSpeed = 5f;

        while (cinemachineComponent.Damping.x > 0.01f)
        {
            cinemachineComponent.Damping.x = Mathf.Lerp(cinemachineComponent.Damping.x, 0f, dampingSpeed * Time.deltaTime);
            yield return null;
        }

        cinemachineComponent.Damping.x = 0f;
        coroutineReference = null;
    }
}
