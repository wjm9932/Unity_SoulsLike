using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFSM
{
    public class SprintState : PlayerMovementState
    {
        public SprintState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            moveSpeed = sm.owner.sprintSpeed;
        }

        public override void Update()
        {
            sm.owner.playerEvents.Sprint(Time.deltaTime);

            if (sm.owner.input.isSprinting == false)
            {
                if (CameraStateMachine.Instance.currentState == CameraStateMachine.Instance.cameraLockOnState)
                {
                    sm.ChangeState(sm.lockOnWalkState);
                }
                else
                {
                    sm.ChangeState(sm.walkState);
                }
            }
            else
            {
                base.Update();
            }

            sm.owner.cameraTransform.localPosition = Vector3.Lerp(sm.owner.cameraTransform.localPosition, sm.owner.originCameraTrasform + sm.owner.transform.forward * -1f, 2f * Time.deltaTime);
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
        }
    }
}
