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
            moveSpeed = 4f;
            sm.owner.rb.velocity = Vector3.zero;
            isDodgeFinished = false;

            sm.owner.animator.SetBool("IsDodging", true);

            SetMoveDirection();

            Dodge();

        }
        public override void Update()
        {
            //SpeedControl();

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
        }
        public override void LateUpdate()
        {

        }
        public override void Exit()
        {
            sm.owner.animator.SetBool("IsDodging", false);
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
                    sm.owner.rb.AddForce(dir * moveSpeed, ForceMode.Impulse);
                }
                else
                {
                    sm.owner.rb.AddForce(GetSlopeMoveDirection().normalized * moveSpeed, ForceMode.Impulse);
                }
            }
            else
            {
                sm.owner.rb.AddForce(sm.owner.transform.forward * moveSpeed, ForceMode.Impulse);
            }
        }

    }
}
