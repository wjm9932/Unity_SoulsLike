using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EnemyFSM
{
    public class IdleState : BossEnemyPatternState
    {
        public IdleState(BossEnemyBehaviorStateMachine sm) : base(sm)
        {
            stoppingDistance = 2f;
            agentSpeed = 4f;
        }

        public override void Enter()
        {
            sm.owner.navMesh.isStopped = false;
            sm.owner.navMesh.speed = agentSpeed;
            sm.owner.navMesh.stoppingDistance = stoppingDistance;
        }
        public override void Update()
        {
            if (Vector3.Distance(sm.character.transform.position, sm.owner.transform.position) >= stoppingDistance)
            {
                sm.owner.transform.rotation = Quaternion.Slerp(sm.owner.transform.rotation, GetMoveRotationAngle(), Time.deltaTime * 5);
                sm.owner.navMesh.SetDestination(sm.character.transform.position);
            }
            else
            {
                sm.ChangeState(sm.swordAttackState);
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
            sm.owner.navMesh.isStopped = true;
            sm.owner.navMesh.ResetPath();
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
            Vector3 direction = sm.owner.navMesh.velocity;
            direction.y = 0; 

            return Quaternion.LookRotation(direction);
        }
    }
}
