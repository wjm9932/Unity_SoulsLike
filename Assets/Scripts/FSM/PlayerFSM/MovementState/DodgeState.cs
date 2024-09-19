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

            sm.owner.animator.SetTrigger("IsRolling");

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
                    sm.ChangeState(sm.walkState);
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
            sm.owner.animator.SetTrigger("DodgeIsDone");
        }
        public override void OnAnimationEnterEvent()
        {
            sm.owner.canBeDamaged = false;
        }
        public override void OnAnimationExitEvent()
        {
            isDodgeFinished = true;
            sm.owner.canBeDamaged = true;
        }
        public override void OnAnimationTransitionEvent()
        {
        }
        private void Dodge()
        {
            sm.owner.transform.LookAt(sm.owner.transform.position + moveDirection);

            if (sm.owner.IsOnSlope() == true)
            {
                if (sm.owner.input.dodgeInput == Vector2.zero)
                {
                    var dir = Vector3.ProjectOnPlane(sm.owner.transform.forward, sm.owner.slopeHit.normal).normalized;
                    sm.owner.rb.AddForce(dir * moveSpeed * 10f, ForceMode.Force);
                }
                else
                {
                    sm.owner.rb.AddForce(GetSlopeMoveDirection().normalized * moveSpeed * 10f, ForceMode.Force);
                }
            }
            else
            {
                sm.owner.rb.AddForce(sm.owner.transform.forward * moveSpeed * 10f, ForceMode.Force);
            }
        }

    }
}
