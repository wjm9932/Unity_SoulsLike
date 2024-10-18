using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace PlayerFSM
{
    public class Combo_1AttackState : AttackState
    {

        public Combo_1AttackState(PlayerMovementStateMachine sm) : base(sm)
        {
            staminaCost = 10;
            dashForce = 1.5f;
        }

        public override void Enter()
        {
            if(sm.owner.UseStamina(staminaCost) == true)
            {
                base.Enter();

                sm.owner.animator.SetTrigger("IsAttack1");
                sm.owner.SetDamage(10f);
            }
            else
            {
                sm.ChangeState(sm.walkState);
            }
        }

        public override void Update()
        {
            if (canComboAttack == true && sm.owner.input.isAttack == true)
            {
                if(sm.owner.uiStateMachine.currentState is InteractStateUI == false)
                {
                    sm.ChangeState(sm.combo_2AttackState);
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
        public override void OnAnimatorIK()
        {
            base.OnAnimatorIK();
        }
    }
}
