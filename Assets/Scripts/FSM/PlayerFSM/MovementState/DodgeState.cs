using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerFSM
{
    public class DodgeState : PlayerMovementState
    {
        private bool isDodgeFinished;
        public DodgeState(PlayerMovementStateMachine sm) : base(sm)
        {

        }
        public override void Enter()
        {
            moveSpeed = 3f;
            isDodgeFinished = false;

            sm.character.animator.SetTrigger("IsRolling");

            SetMoveDirection();
        }
        public override void Update()
        {
            SpeedControl();

            if (isDodgeFinished == true)
            {
                if (CameraStateMachine.Instance.currentState == CameraStateMachine.Instance.cameraLockOnState)
                {
                    sm.ChangeState(sm.lockOnWalkState);
                }
                else
                {
                    sm.ChangeState(sm.idleState);
                }
            }
        }
        public override void PhysicsUpdate()
        {
            Dodge();
        }
        public override void LateUpdate()
        {

        }
        public override void Exit()
        {
        }
        public override void OnAnimationEnterEvent()
        {
            sm.character.canBeDamaged = false;
        }
        public override void OnAnimationExitEvent()
        {
            isDodgeFinished = true;
            sm.character.canBeDamaged = true;
        }
        public override void OnAnimationTransitionEvent()
        {
        }
        private void Dodge()
        {
            sm.character.transform.LookAt(sm.character.transform.position + moveDirection);

            if (sm.character.IsOnSlope() == true)
            {
                if (sm.character.input.dodgeInput == Vector2.zero)
                {
                    var dir = Vector3.ProjectOnPlane(sm.character.transform.forward, sm.character.slopeHit.normal).normalized;
                    sm.character.rb.AddForce(dir * moveSpeed * 10f, ForceMode.Force);
                }
                else
                {
                    sm.character.rb.AddForce(GetSlopeMoveDirection().normalized * moveSpeed * 10f, ForceMode.Force);
                }
            }
            else
            {
                sm.character.rb.AddForce(sm.character.transform.forward * moveSpeed * 10f, ForceMode.Force);
            }
        }

    }
}
