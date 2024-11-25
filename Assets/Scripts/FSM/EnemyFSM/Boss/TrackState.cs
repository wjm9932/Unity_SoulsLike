using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BossEnemyFSM
{
    public class TrackState : BossEnemyPatternState
    {
        public TrackState(BossEnemyBehaviorStateMachine sm) : base(sm)
        {
            stoppingDistance = 2f;
            agentSpeed = 4f;
        }

        public override void Enter()
        {
            if(sm.owner.hpBar.gameObject.activeSelf == false)
            {
                sm.owner.hpBar.gameObject.SetActive(true);
            }

            sm.owner.navMesh.isStopped = false;
            sm.owner.navMesh.speed = agentSpeed;
            sm.owner.navMesh.stoppingDistance = stoppingDistance;
            sm.owner.animator.SetFloat("Speed", sm.owner.navMesh.speed);
        }
        public override void Update()
        {
            if (Vector3.Distance(sm.owner.target.transform.position, sm.owner.transform.position) >= stoppingDistance)
            {
                sm.owner.transform.rotation = Quaternion.Slerp(sm.owner.transform.rotation, GetMoveRotationAngle(), Time.deltaTime * 5);
                sm.owner.navMesh.SetDestination(sm.owner.target.transform.position);
            }
            else
            {
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
        private void GetBossPattern()
        {
            int pattern = Random.Range(0, 4);
            switch (pattern)
            {
                case 0:
                    sm.ChangeState(sm.stabAttackState);
                    break;
                case 1:
                    sm.ChangeState(sm.jumpAttackState);
                    break;
                case 2:
                    sm.ChangeState(sm.swordAttackState);
                    break;
                case 3:
                    sm.ChangeState(sm.backFlipState);
                    break;
                default:
                    break;
            }
        }
    }
}
