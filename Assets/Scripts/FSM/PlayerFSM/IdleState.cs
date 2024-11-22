using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFSM
{
    public class IdleState : IState
    {
        private PlayerMovementStateMachine sm;
        public IdleState(PlayerMovementStateMachine playerMovementStateMachine)
        {
            sm = playerMovementStateMachine;
        }
        public void Enter()
        {
            sm.owner.rb.velocity = Vector3.zero;
        }
        public void Update()
        {
            sm.owner.cameraTransform.localPosition = Vector3.Lerp(sm.owner.cameraTransform.localPosition, sm.owner.originCameraTrasform , 2f * Time.deltaTime);
            //if(sm.owner.IsOnSlope() == true)
            //{
            //    sm.owner.rb.velocity = Vector3.zero;
            //}

            if (sm.owner.input.moveInput != Vector2.zero)
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


            if (sm.owner.input.IsClickItemInInventory(sm.owner.OnClickItem) == true)
            {
                sm.ChangeState(sm.owner.toBeUsedItem.GetTargetState(sm));
            }

            if(sm.owner.input.isUsingQuickSlot == true)
            {
                if(sm.owner.quickSlot != null)
                {
                    sm.ChangeState(sm.owner.quickSlot.GetTargetState(sm));
                }
            }

            if (sm.owner.uiStateMachine.currentState is InteractStateUI == true)
            {
                sm.ChangeState(sm.interactState);
            }

            if (sm.owner.uiStateMachine.currentState is OpenState == false)
            {
                if (sm.owner.input.isDodging == true)
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
                if (sm.owner.input.isAttack == true)
                {
                    sm.ChangeState(sm.combo_1AttackState);
                }
                if(sm.owner.input.isChargingStart == true)
                {
                    sm.ChangeState(sm.chargingState);
                }
            }
        }
        public void PhysicsUpdate()
        {
            //base.PhysicsUpdate();
        }
        public void LateUpdate()
        {
            //base.LateUpdate();
            UpdateAnimation();
        }
        public void Exit()
        {
            //base.Exit();
        }
        public void OnAnimatorIK()
        {
            sm.owner.animator.SetFloat("HandWeight", 1, 0.1f, Time.deltaTime * 0.1f);
            sm.owner.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, sm.owner.animator.GetFloat("HandWeight"));
            sm.owner.animator.SetIKPosition(AvatarIKGoal.LeftHand, sm.owner.leftHandPos.position);
        }
        void UpdateAnimation()
        {
            sm.owner.animator.SetFloat("Speed", sm.owner.rb.velocity.magnitude, 0.08f, Time.deltaTime);
        }
        public virtual void OnAnimationEnterEvent()
        {

        }
        public virtual void OnAnimationExitEvent()
        {
        }
        public virtual void OnAnimationTransitionEvent()
        {
        }
    }
}


