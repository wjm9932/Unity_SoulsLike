using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFSM
{
    public class SwordAttackState : IState
    {
        protected EnemyBehaviorStateMachine sm;
        public SwordAttackState(EnemyBehaviorStateMachine sm)
        {
            this.sm = sm;
        }

        public virtual void Enter()
        {
            sm.enemy.navMesh.isStopped = true;
            sm.enemy.animator.SetTrigger("Attack");
        }
        public virtual void Update()
        {
            if (sm.enemy.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f && sm.enemy.animator.IsInTransition(0) == false)
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

        }
        public virtual void OnAnimationTransitionEvent()
        {

        }
        public virtual void OnAnimatorIK()
        {

        }
    }
}
