using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EnemyFSM
{
    public class IdleState : IState
    {
        protected EnemyBehaviorStateMachine sm;
        public IdleState(EnemyBehaviorStateMachine sm)
        {
            this.sm = sm;
        }

        public virtual void Enter()
        {

        }
        public virtual void Update()
        {
            Rotate();
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
        private void Rotate()
        {
            Vector3 direction = sm.character.transform.position - sm.enemy.transform.position;
            direction.y = 0;

            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            sm.enemy.transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}
