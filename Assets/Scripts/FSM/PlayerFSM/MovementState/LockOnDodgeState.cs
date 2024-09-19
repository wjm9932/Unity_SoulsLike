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
            moveSpeed = 2f;
            dodgeDir = sm.owner.input.dodgeInput;

            UpdateAnimation();
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
                    moveDir = sm.owner.transform.forward * dodgeDir.y + sm.owner.transform.right * dodgeDir.x;
                }
                sm.owner.rb.AddForce(moveDir.normalized * moveSpeed, ForceMode.Impulse);
            }
        }

        protected override Vector3 GetSlopeMoveDirection()
        {
            Vector3 moveDir = sm.owner.transform.forward * dodgeDir.y + sm.owner.transform.right * dodgeDir.x;
            return Vector3.ProjectOnPlane(moveDir, sm.owner.slopeHit.normal).normalized;
        }

        //private void Rotate()
        //{
        //    Vector3 direction = new Vector3(sm.character.tempTarget.transform.position.x - sm.character.rb.position.x, 0, sm.character.tempTarget.transform.position.z - sm.character.rb.position.z);

        //    Quaternion targetRotation = Quaternion.LookRotation(direction);
        //    sm.character.rb.MoveRotation(Quaternion.Slerp(sm.character.rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));
        //}

        //IEnumerator PostSimulationUpdate()
        //{
        //    YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        //    while (true)
        //    {
        //        yield return waitForFixedUpdate;
        //        Rotate();
        //    }
        //}
        private void UpdateAnimation()
        {
            sm.owner.animator.SetFloat("Horizontal", dodgeDir.x);
            sm.owner.animator.SetFloat("Vertical", dodgeDir.y);
            sm.owner.animator.SetTrigger("IsRolling");
        }
    }
}
