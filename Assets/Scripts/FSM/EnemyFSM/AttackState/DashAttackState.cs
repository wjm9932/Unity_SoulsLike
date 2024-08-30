using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFSM
{
    public class DashAttackState : EnemyPatternState
    {
        private float distance;
        private float timer;
        public DashAttackState(EnemyBehaviorStateMachine sm) : base(sm)
        {
            stoppingDistance = 0f;
        }

        public override void Enter()
        {
            timer = 0.1f;

            distance = Vector3.Distance(sm.enemy.transform.position, sm.character.transform.position);
            agentSpeed = distance / 0.15f;

            sm.enemy.navMesh.stoppingDistance = stoppingDistance;
            sm.enemy.navMesh.speed = agentSpeed;
            SetDashDestination();

            dir = GetLookAtAngle();
            sm.enemy.animator.SetTrigger("DashStab");
            sm.enemy.GetComponent<CapsuleCollider>().isTrigger = true;
            sm.enemy.attack.SetCanAttack(1);
        }
        public override void Update()
        {
            sm.enemy.transform.rotation = Quaternion.Slerp(sm.enemy.transform.rotation, dir, Time.deltaTime * 10);
            if(sm.enemy.navMesh.remainingDistance <= 0f)
            {
                timer -= Time.deltaTime;
            }
            if(timer <= 0)
            {
                sm.enemy.animator.SetTrigger("StabDone");
                GetBossPattern();
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
            sm.enemy.GetComponent<CapsuleCollider>().isTrigger = false;
            sm.enemy.attack.SetCanAttack(0);
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
            Vector3 backOffset = sm.enemy.transform.forward * 5f;
            Vector3 dashDestination = sm.character.transform.position + backOffset;

            sm.enemy.navMesh.SetDestination(dashDestination);
        }
    }
}
