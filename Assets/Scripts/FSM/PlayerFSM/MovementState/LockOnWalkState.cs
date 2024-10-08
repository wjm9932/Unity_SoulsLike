using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFSM
{
    public class LockOnWalkState : PlayerMovementState
    {
        private IEnumerator coroutineReference;
        public LockOnWalkState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            coroutineReference = PostSimulationUpdate();
        }

        public override void Enter()
        {
            sm.owner.StartCoroutine(coroutineReference);
            moveSpeed = sm.owner.walkSpeed;
        }

        public override void Update()
        {
            if (sm.owner.input.isSprinting == true)
            {
                sm.ChangeState(sm.sprintState);
            }
            else if (CameraStateMachine.Instance.currentState == CameraStateMachine.Instance.cameraLockOffState)
            {
                sm.ChangeState(sm.walkState);
            }
            else
            {
                base.Update();
            }
        }
        public override void PhysicsUpdate()
        {
            Move();
        }
        public override void LateUpdate()
        {
            base.LateUpdate();
        }
        public override void Exit()
        {
            sm.owner.StopCoroutine(coroutineReference);
        }
        private void Rotate()
        {
            //Vector3 direction = new Vector3(CameraStateMachine.Instance.cameraLockOnState.target.transform.position.x - sm.owner.rb.position.x, 0, CameraStateMachine.Instance.cameraLockOnState.target.transform.position.z - sm.owner.rb.position.z);
            Vector3 direction = sm.owner.mainCamera.transform.forward;
            direction.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            sm.owner.rb.MoveRotation(Quaternion.Slerp(sm.owner.rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));
        }
        protected override void Move()
        {
            if (sm.owner.IsOnSlope() == true)
            {
                sm.owner.rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);
            }
            else
            {
                sm.owner.rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
        }
        protected override Vector3 GetSlopeMoveDirection()
        {
            Vector3 moveDir = sm.owner.transform.forward * sm.owner.input.moveInput.y + sm.owner.transform.right * sm.owner.input.moveInput.x;
            return Vector3.ProjectOnPlane(moveDir, sm.owner.slopeHit.normal).normalized;
        }
        IEnumerator PostSimulationUpdate()
        {
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
            while (true)
            {
                yield return waitForFixedUpdate;
                Rotate();
            }
        }
    }
}
