using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFSM
{
    public class DashAttackState : EnemyPatternState
    {
        public DashAttackState(EnemyBehaviorStateMachine sm) : base(sm)
        {
            stoppingDistance = 0f;
        }

        public override void Enter()
        {
            sm.enemy.navMesh.stoppingDistance = stoppingDistance;
            sm.enemy.navMesh.speed = 20f;
            SetDashDestination();

            dir = GetLookAtAngle();
        }
        public override void Update()
        {
            sm.enemy.transform.rotation = Quaternion.Slerp(sm.enemy.transform.rotation, dir, Time.deltaTime * 10);
        }
        public override void PhysicsUpdate()
        {

        }
        public override void LateUpdate()
        {

        }
        public override void Exit()
        {
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
        private void GetBossPattern()
        {
            int pattern = Random.Range(0, 2);
            switch (pattern)
            {
                case 0:
                    sm.ChangeState(sm.idleState);
                    break;
                case 1:
                    sm.ChangeState(sm.jumpAttackState);
                    break;
                default:
                    break;
            }
        }
        private void SetDashDestination()
        {
            Vector3 backOffset = sm.enemy.transform.forward * 5;
            Vector3 dashDestination = sm.character.transform.position + backOffset;

            sm.enemy.navMesh.SetDestination(dashDestination);
        }
    }
}
