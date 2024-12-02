using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerFSM
{
    public class DieState : IState
    {
        protected PlayerMovementStateMachine sm;
        public DieState(PlayerMovementStateMachine sm)
        {
            this.sm = sm;
        }
        public virtual void Enter()
        {
            sm.owner.canAttack = false;
            sm.owner.canBeDamaged = false;
            sm.owner.rb.velocity = Vector3.zero;
            sm.owner.animator.SetBool("IsDie", true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (sm.owner.playerCameraStateMachine.currentState == sm.owner.playerCameraStateMachine.cameraLockOnState)
            {
                sm.owner.playerCameraStateMachine.ChangeState(sm.owner.playerCameraStateMachine.cameraLockOffState);
            }

            if (SoundManager.Instance.drinkAudioSource != null)
            {
                SoundManager.Instance.drinkAudioSource.Stop();
            }
            SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.PLAYER_DIE, 0.3f);
        }
        public virtual void Update()
        {
            sm.owner.rb.velocity = Vector3.zero;
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
            sm.owner.animator.SetBool("IsDie", false);


            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
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
        public virtual void OnAnimatorIK()
        {
            sm.owner.animator.SetFloat("HandWeight", 0);
        }
    }
}
