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
            sm.enemy.navMesh.isStopped = false;
        }
        public virtual void Update()
        {
            sm.enemy.transform.rotation = Quaternion.Slerp(sm.enemy.transform.rotation, GetAngle(), Time.deltaTime * 5);

            if (Vector3.Distance(sm.character.transform.position, sm.enemy.transform.position) >= 3f)
            {
                sm.enemy.navMesh.SetDestination(sm.character.transform.position);
            }
            else
            {
                if (Quaternion.Angle(sm.enemy.transform.rotation, GetAngle()) < 10f)
                {
                    sm.ChangeState(sm.SwordAttackState);
                }
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
            sm.enemy.navMesh.isStopped = true;
            sm.enemy.navMesh.ResetPath();
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
        private Quaternion GetAngle()
        {
            Vector3 direction = sm.character.transform.position - sm.enemy.transform.position;
            direction.y = 0; 

            return Quaternion.LookRotation(direction);
        }
    }
}
