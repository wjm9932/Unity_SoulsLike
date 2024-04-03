using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

namespace PlayerFSM
{
    public class WalkSate : PlayerMovementState
    {
        public WalkSate(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            moveSpeed = 3f;
        }

        public override void Update()
        {
            if (sm.character.input.isSprinting == true)
            {
                sm.ChangeState(sm.sprintState);
            }
            else if (CameraStateMachine.Instance.currentState == CameraStateMachine.Instance.cameraLockOnState)
            {
                sm.ChangeState(sm.lockOnWalkState);
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
        public override void OnAnimatorIK()
        {
            sm.character.animator.SetFloat("HandWeight", 1, 0.1f, Time.deltaTime * 0.1f);
            sm.character.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, sm.character.animator.GetFloat("HandWeight"));
            sm.character.animator.SetIKPosition(AvatarIKGoal.LeftHand, sm.character.leftHandPos.position);
        }
    }
}


