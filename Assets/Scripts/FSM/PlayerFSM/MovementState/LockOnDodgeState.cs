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
            sm.character.rb.velocity = Vector3.zero;
            moveSpeed = 2f;
            dodgeDir = sm.character.input.dodgeInput;
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
            //sm.character.rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            if (sm.character.IsOnSlope() == true)
            {
                sm.character.rb.AddForce(GetSlopeMoveDirection().normalized * moveSpeed, ForceMode.Impulse);
            }
            else
            {
                if (dodgeDir == Vector2.zero)
                {
                    moveDir = sm.character.transform.forward;
                }
                else
                {
                    moveDir = sm.character.transform.forward * dodgeDir.y + sm.character.transform.right * dodgeDir.x;
                }
                sm.character.rb.AddForce(moveDir.normalized * moveSpeed, ForceMode.Impulse);
            }
        }

        protected override Vector3 GetSlopeMoveDirection()
        {
            Vector3 moveDir = sm.character.transform.forward * dodgeDir.y + sm.character.transform.right * dodgeDir.x;
            return Vector3.ProjectOnPlane(moveDir, sm.character.slopeHit.normal).normalized;
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
            sm.character.animator.SetFloat("Horizontal", dodgeDir.x);
            sm.character.animator.SetFloat("Vertical", dodgeDir.y);
            sm.character.animator.SetTrigger("IsRolling");
        }
    }
}
