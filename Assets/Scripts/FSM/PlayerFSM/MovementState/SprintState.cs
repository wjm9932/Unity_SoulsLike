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

            sm.owner.cameraTransform.localPosition = Vector3.Lerp(sm.owner.cameraTransform.localPosition, 
                sm.owner.originCameraTrasform + sm.owner.transform.forward * -1.5f, 
                2f * Time.deltaTime);

            if (sm.owner.input.isSprinting == false)
            {
                if (sm.owner.playerCameraStateMachine.currentState == sm.owner.playerCameraStateMachine.cameraLockOnState)
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
