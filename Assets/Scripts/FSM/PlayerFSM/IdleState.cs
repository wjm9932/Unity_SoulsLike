using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFSM
{
    public class IdleState : PlayerMovementState
    {
        public IdleState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            //base.Enter();
            sm.character.rb.velocity = Vector3.zero;
        }
        public override void Update()
        {
            if(Input.GetKeyDown(KeyCode.K))
            {
                sm.character.animator.SetTrigger("DrinkPotion");
            }
            if (sm.character.IsOnSlope() == true)
            {
                sm.character.rb.velocity = Vector3.zero;
            }
            if (sm.character.input.moveInput != Vector2.zero)
            {
                if (CameraStateMachine.Instance.currentState == CameraStateMachine.Instance.cameraLockOffState)
                {
                    sm.ChangeState(sm.walkState);
                }
                else
                {
                    sm.ChangeState(sm.lockOnWalkState);
                }
            }
            if (sm.uiStatMachine.currentState is OpenState == false)
            {
                if (sm.character.input.isDodging == true)
                {
                    if (CameraStateMachine.Instance.currentState == CameraStateMachine.Instance.cameraLockOnState)
                    {
                        sm.ChangeState(sm.lockOnDodgeState);
                    }
                    else
                    {
                        sm.ChangeState(sm.dodgeState);
                    }
                }
                if (sm.character.input.isAttack == true)
                {
                    sm.ChangeState(sm.combo_1AttackState);
                }
            }
        }
        public override void PhysicsUpdate()
        {
            //base.PhysicsUpdate();
        }
        public override void LateUpdate()
        {
            //base.LateUpdate();
            UpdateAnimation();
        }
        public override void Exit()
        {
            //base.Exit();
        }
        public override void OnAnimatorIK()
        {
            sm.character.animator.SetFloat("HandWeight", 1, 0.1f, Time.deltaTime * 0.1f);
            sm.character.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, sm.character.animator.GetFloat("HandWeight"));
            sm.character.animator.SetIKPosition(AvatarIKGoal.LeftHand, sm.character.leftHandPos.position);
        }
        void UpdateAnimation()
        {
            sm.character.animator.SetFloat("Speed", sm.character.rb.velocity.magnitude, 0.08f, Time.deltaTime);
        }

    }
}


