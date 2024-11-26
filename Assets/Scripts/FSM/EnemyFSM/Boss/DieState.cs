using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace BossEnemyFSM
{
    public class DieState : IState
    {
        protected BossEnemyBehaviorStateMachine sm;
        public DieState(BossEnemyBehaviorStateMachine sm)
        {
            this.sm = sm;
        }

        public virtual void Enter()
        {
            sm.owner.canAttack = false;
            SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.ENEMY_DIE, 0.2f);
            sm.owner.animator.SetTrigger("Die");
        }
        public virtual void Update()
        {
            if (sm.owner.canAttack == true)
            {
                StackTrace stackTrace = new StackTrace(true);
                UnityEngine.Debug.Log(stackTrace.ToString());
                UnityEngine.Debug.LogError(sm.owner.name + "is in Die state: " + sm.owner.canAttack);
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

        }
        public virtual void OnAnimationTransitionEvent()
        {

        }
        public virtual void OnAnimatorIK()
        {

        }
    }
}
