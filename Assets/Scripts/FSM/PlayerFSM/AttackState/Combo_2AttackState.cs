using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFSM
{
    public class Combo_2AttackState : AttackState
    {
        public Combo_2AttackState(PlayerMovementStateMachine sm) : base(sm)
        {
        }
        public override void Enter()
        {
            dashForce = 1.5f;
            sm.owner.animator.SetTrigger("IsAttack2");
            canComboAttack = false;
            sm.owner.SetDamage(20f);

            base.Enter();
        }

        public override void Update()
        {
            if (canComboAttack == true && sm.owner.input.isAttack == true)
            {
                if (sm.owner.uiStateMachine.currentState is InteractStateUI == false)
                {
                    sm.ChangeState(sm.combo_3AttackState);
                }
            }
            else
            {
                base.Update();
            }
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        public override void LateUpdate()
        {
            base.LateUpdate();
        }
        public override void Exit()
        {
            base.Exit();
        }

        public override void OnAnimationEnterEvent()
        {
            base.OnAnimationEnterEvent();
        }
        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();
        }
        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();
        }
    }
}
