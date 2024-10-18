using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFSM
{
    public class Combo_3AttackState : AttackState
    {
        public Combo_3AttackState(PlayerMovementStateMachine sm) : base(sm)
        {
            staminaCost = 30f;
            dashForce = 6f;
        }
        public override void Enter()
        {
            if (sm.owner.UseStamina(staminaCost) == true)
            {
                base.Enter();

                sm.owner.staminaRecoverCoolTime = Character.targetStaminaRecoverCoolTime;
                sm.owner.animator.SetTrigger("IsAttack3");
                sm.owner.SetDamage(30f);
            }
            else
            {
                rotation = sm.owner.transform.rotation;
            }
        }

        public override void Update()
        {
            base.Update();
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