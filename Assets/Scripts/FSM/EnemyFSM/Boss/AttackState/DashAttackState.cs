using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossEnemyFSM
{
    public class DashAttackState : BossEnemyPatternState
    {
        private float distance;
        private float timer;
        public DashAttackState(BossEnemyBehaviorStateMachine sm) : base(sm)
        {
            stoppingDistance = 0f;
        }

        public override void Enter()
        {
            timer = 0.1f;

            distance = Vector3.Distance(sm.owner.transform.position, sm.owner.target.transform.position);
            agentSpeed = distance / 0.15f;

            sm.owner.navMesh.stoppingDistance = stoppingDistance;
            sm.owner.navMesh.speed = agentSpeed;
            SetDashDestination();

            dir = GetLookAtAngle();
            sm.owner.animator.SetBool("isDashStab", true);
            sm.owner.GetComponent<CapsuleCollider>().isTrigger = true;
            sm.owner.SetCanAttack(1);
            sm.owner.SetDamage(20f);
        }
        public override void Update()
        {
            sm.owner.transform.rotation = Quaternion.Slerp(sm.owner.transform.rotation, dir, Time.deltaTime * 10);
            if(sm.owner.navMesh.remainingDistance <= 0f)
            {
                timer -= Time.deltaTime;
            }
            if(timer <= 0)
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
            sm.owner.animator.SetBool("isDashStab", false);
            sm.owner.GetComponent<CapsuleCollider>().isTrigger = false;
            sm.owner.SetCanAttack(0);
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
                    sm.ChangeState(sm.trackingState);
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
            Vector3 backOffset = sm.owner.transform.forward * 5f;
            Vector3 dashDestination = sm.owner.target.transform.position + backOffset;

            sm.owner.navMesh.SetDestination(dashDestination);
        }
    }
}
