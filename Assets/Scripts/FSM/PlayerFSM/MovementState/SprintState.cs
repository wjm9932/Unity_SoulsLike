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
            moveSpeed = 5f;
        }

        public override void Update()
        {
            if (sm.character.input.isSprinting == false)
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
