using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace EnemyFSM
{
    public class BackFlipState : EnemyPatternState
    {
        private float distance;
        public BackFlipState(EnemyBehaviorStateMachine sm) : base(sm)
        {
            stoppingDistance = 0f;
        }

        public override void Enter()
        {
            dir = GetLookAtAngle();

            SetDashDestinationAndSpeed();
            sm.enemy.animator.SetTrigger("BackFlip");
        }
        public override void Update()
        {
            sm.enemy.transform.rotation = Quaternion.Slerp(sm.enemy.transform.rotation, dir, Time.deltaTime * 30);

            if (sm.enemy.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f && sm.enemy.animator.IsInTransition(0) == false)
            {
                if (sm.enemy.isTest == true)
                {
                    sm.ChangeState(sm.idleState);
                }
                else
                {
                    GetBossPattern();
                }
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
                    sm.ChangeState(sm.stanbyStabAttackState);
                    break;
                case 1:
                    sm.ChangeState(sm.jumpAttackState);
                    break;
                default:
                    break;
            }
        }

        private void SetDashDestinationAndSpeed()
        {
            Vector3 forwardDirection = dir * Vector3.forward;
            Vector3 backOffset = forwardDirection * -5;
            Vector3 dashDestination = sm.enemy.transform.position + backOffset;

            sm.enemy.navMesh.SetDestination(dashDestination);

            distance = Vector3.Distance(sm.enemy.transform.position, dashDestination);
            agentSpeed = distance / 0.75f;
            sm.enemy.navMesh.speed = agentSpeed;
        }
    }
}
