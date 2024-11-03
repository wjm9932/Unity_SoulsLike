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
    public CameraLockOffState(CameraStateMachine CameraStateMachine) : base(CameraStateMachine)
    {
        bottonClamp = -30f;
        topClamp = 35f;
    }
    public override void Enter()
    {
        base.Enter();
        csm.owner.animator.SetBool("IsLockOn", false);

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
        //base.Exit();
    }

    private void CameraRotation()
    {

        // if there is an input and camera position is not fixed
        cameraTargetYaw += csm.owner.input.mouseInput.x * 1.5f;
        cameraTargetPitch -= csm.owner.input.mouseInput.y;

        // clamp our rotations so our values are limited 360 degrees
        cameraTargetYaw = ClampAngle(cameraTargetYaw, float.MinValue, float.MaxValue);
        cameraTargetPitch = ClampAngle(cameraTargetPitch, bottonClamp, topClamp);

        // Cinemachine will follow this target
        csm.owner.cameraTransform.rotation = Quaternion.Euler(cameraTargetPitch,
            cameraTargetYaw, 0.0f);
    }
    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
