using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BossEnemyFSM
{
    public class BackFlipState : BossEnemyPatternState
    {
        private float distance;
        public BackFlipState(BossEnemyBehaviorStateMachine sm) : base(sm)
        {
            stoppingDistance = 0f;
        }

        public override void Enter()
        {
            sm.owner.navMesh.stoppingDistance = stoppingDistance;

            dir = GetLookAtAngle();
            SetDashDestinationAndSpeed();
            sm.owner.animator.SetBool("isBackFlip", true);
        }
        public override void Update()
        {
            sm.owner.transform.rotation = Quaternion.Slerp(sm.owner.transform.rotation, dir, Time.deltaTime * 30);

            if (sm.owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f && sm.owner.animator.IsInTransition(0) == false)
            {
                GetBossPattern();
            }
            base.Update();
        }
        public override void PhysicsUpdate()
        {

        }
        public override void LateUpdate()
        {

        }
        public override void Exit()
        {
            sm.owner.animator.SetBool("isBackFlip", false);
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
            Vector3 dashDestination = sm.owner.transform.position + backOffset;

            sm.owner.navMesh.SetDestination(dashDestination);

            distance = Vector3.Distance(sm.owner.transform.position, dashDestination);
            agentSpeed = distance / 0.75f;
            sm.owner.navMesh.speed = agentSpeed;
        }
    }
}
