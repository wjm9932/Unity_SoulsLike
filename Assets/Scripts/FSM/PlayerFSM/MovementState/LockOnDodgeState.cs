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
        private Vector3 forward;
        private Vector3 right;
        public LockOnDodgeState(PlayerMovementStateMachine sm) : base(sm)
        {
        }
        public override void Enter()
        {
            sm.owner.canBeDamaged = false;
            isDodgeFinished = false;
            forward = sm.owner.mainCamera.transform.forward;
            right = sm.owner.mainCamera.transform.right;
            moveSpeed = 2f;
            dodgeDir = sm.owner.input.dodgeInput;
            
            UpdateAnimation();

            SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.DODGE);
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
            sm.owner.animator.SetBool("IsDodging", false);
        }
        public override void OnAnimationEnterEvent()
        {
            //sm.owner.canBeDamaged = false;
        }
        public override void OnAnimationExitEvent()
        {
            isDodgeFinished = true;
            sm.owner.canBeDamaged = true;
        }
        public override void OnAnimationTransitionEvent()
        {
            SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.DODGE_LANDING, 1f);
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
                    moveDir = forward * dodgeDir.y + right * dodgeDir.x;
                }
                sm.owner.rb.AddForce(moveDir.normalized * moveSpeed, ForceMode.Impulse);
            }
        }

        protected override Vector3 GetSlopeMoveDirection()
        {
            Vector3 moveDir = forward * dodgeDir.y + right * dodgeDir.x;
            return Vector3.ProjectOnPlane(moveDir, sm.owner.slopeHit.normal).normalized;
        }

        private void UpdateAnimation()
        {
            Vector3 dir = forward * dodgeDir.y + right * dodgeDir.x;
            Vector3 localMoveDir = sm.owner.transform.InverseTransformDirection(dir).normalized;

            sm.owner.animator.SetFloat("Horizontal", localMoveDir.x);
            sm.owner.animator.SetFloat("Vertical", localMoveDir.z);
            sm.owner.animator.SetBool("IsDodging", true);
        }
    }
}
