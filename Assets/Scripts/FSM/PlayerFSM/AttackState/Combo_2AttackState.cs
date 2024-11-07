using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFSM
{
    public class Combo_2AttackState : AttackState
    {
        public Combo_2AttackState(PlayerMovementStateMachine sm) : base(sm)
        {
            staminaCost = 20f;
            dashForce = 1.5f;
        }
        public override void Enter()
        {
            if (sm.owner.UseStamina(staminaCost) == true)
            {
                base.Enter();

                sm.owner.playerEvents.Attack("Second");
                sm.owner.staminaRecoverCoolTime = Character.targetStaminaRecoverCoolTime;
                sm.owner.animator.SetTrigger("IsAttack2");
                sm.owner.SetDamage(20f);
            }
            else
            {
                rotation = sm.owner.transform.rotation;
            }
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
            SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.ATTACK_2, 0.4f);
        }
    }
}
