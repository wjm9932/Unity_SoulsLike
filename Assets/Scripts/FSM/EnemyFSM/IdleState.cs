using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EnemyFSM
{
    public class IdleState : EnemyPatternState
    {
        public IdleState(EnemyBehaviorStateMachine sm) : base(sm)
        {
            stoppingDistance = 2f;
            agentSpeed = 4f;
        }

        public override void Enter()
        {
            sm.enemy.navMesh.isStopped = false;
            sm.enemy.navMesh.speed = agentSpeed;
            sm.enemy.navMesh.stoppingDistance = stoppingDistance;
        }
        public override void Update()
        {
            if (Vector3.Distance(sm.character.transform.position, sm.enemy.transform.position) >= stoppingDistance)
            {
                sm.enemy.transform.rotation = Quaternion.Slerp(sm.enemy.transform.rotation, GetMoveRotationAngle(), Time.deltaTime * 5);
                sm.enemy.navMesh.SetDestination(sm.character.transform.position);
            }
            else
            {
                sm.enemy.navMesh.ResetPath();
                sm.ChangeState(sm.swordAttackState);
                //sm.ChangeState(sm.stanbyStabAttackState);
            }
        }
        public override void PhysicsUpdate()
        {

        }
        public override void LateUpdate()
        {

        }
        public override void Exit()
        {
            sm.enemy.navMesh.isStopped = true;
            sm.enemy.navMesh.ResetPath();
        }
        public override void OnAnimationEnterEvent()
        {

        }
        public override void OnAnimationExitEvent()
        {

        }
        public override void OnAnimationTransitionEvent()
        {

        }
        public override void OnAnimatorIK()
        {

        }
        private Quaternion GetMoveRotationAngle()
        {
            Vector3 direction = sm.enemy.navMesh.velocity;
            direction.y = 0; 

            return Quaternion.LookRotation(direction);
        }
    }
}
