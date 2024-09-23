using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFSM
{
    public class LockOnDodgeState : PlayerMovementState
    {
        private bool isDodgeFinished;
        private Vector3 moveDir;
        private Vector2 dodgeDir;

        public LockOnDodgeState(PlayerMovementStateMachine sm) : base(sm)
        {
        }
        public override void Enter()
        {
            isDodgeFinished = false;
            sm.owner.rb.velocity = Vector3.zero;

            moveSpeed = 4f;
            dodgeDir = sm.owner.input.dodgeInput;
            
            Dodge();

            UpdateAnimation();
        }
        public override void Update()
        {
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
            if (sm.owner.IsOnSlope() == true)
            {
                if (dodgeDir == Vector2.zero)
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
                if (dodgeDir == Vector2.zero)
                {
                    moveDir = sm.owner.transform.forward;
                }
                else
                {
                    moveDir = sm.owner.mainCamera.transform.forward * dodgeDir.y + sm.owner.mainCamera.transform.right * dodgeDir.x;
                }
                sm.owner.rb.AddForce(moveDir.normalized * moveSpeed, ForceMode.Impulse);
            }
        }

        protected override Vector3 GetSlopeMoveDirection()
        {
            moveDir = sm.owner.mainCamera.transform.forward * dodgeDir.y + sm.owner.mainCamera.transform.right * dodgeDir.x;
            return Vector3.ProjectOnPlane(moveDir, sm.owner.slopeHit.normal).normalized;
        }

        private void UpdateAnimation()
        {
            Vector3 localMoveDir = sm.owner.transform.InverseTransformDirection(moveDir).normalized;

            sm.owner.animator.SetFloat("Horizontal", localMoveDir.x); 
            sm.owner.animator.SetFloat("Vertical", localMoveDir.z);   
            sm.owner.animator.SetTrigger("IsRolling");
        }
    }
}
