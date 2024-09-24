using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFSM
{
    public class HitState : IState
    {
        protected PlayerMovementStateMachine sm;
        private bool exit;
        public HitState(PlayerMovementStateMachine sm)
        {
            this.sm = sm;
        }
        public virtual void Enter()
        {
            sm.owner.animator.SetBool("IsHit", true);
            exit = false;
            sm.owner.canBeDamaged = false;
            sm.owner.rb.velocity = Vector3.zero;
        }
        public virtual void Update()
        {
            if (exit == true)
            {
                sm.ChangeState(sm.idleState);
            }
        }
        public virtual void PhysicsUpdate()
        {

        }
        public virtual void LateUpdate()
        {

        }
        public virtual void Exit()
        {
            sm.owner.canBeDamaged = true;
            sm.owner.SetCanAttack(0);
            sm.owner.animator.SetBool("IsHit", false);
        }
        public virtual void OnAnimationEnterEvent()
        {

        }
        public virtual void OnAnimationExitEvent()
        {
            exit = true;
        }
        public virtual void OnAnimationTransitionEvent()
        {
        }
        public virtual void OnAnimatorIK()
        {
            sm.owner.animator.SetFloat("HandWeight", 0);
        }
    }
}
