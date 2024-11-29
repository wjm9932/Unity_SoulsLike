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
        private void GetBossPattern()
        {
            int pattern = Random.Range(0, 100);
            if (pattern < 40)
            {
                sm.ChangeState(sm.swordAttackState);
            }
            else if(pattern < 80)
            {
                sm.ChangeState(sm.chargingSwordAttackState);
            }
            else
            {
                sm.ChangeState(sm.backFlipState);
            }
        }
    }
}
