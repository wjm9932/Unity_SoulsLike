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
            exit = false;
            sm.character.rb.velocity = Vector3.zero;
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
            sm.character.animator.SetFloat("HandWeight", 0);
        }
    }
}
