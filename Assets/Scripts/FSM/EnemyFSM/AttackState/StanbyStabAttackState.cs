using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFSM
{
    public class StanbyStabAttackState : EnemyPatternState
    {
        private bool isReadyToAttack;
        public StanbyStabAttackState(BossEnemyBehaviorStateMachine sm) : base(sm)
        {
            stoppingDistance = 0f;
        }

        public override void Enter()
        {
            sm.enemy.StartCoroutine(GetReady());

            isReadyToAttack = false;
            sm.enemy.navMesh.isStopped = true;
            sm.enemy.animator.SetTrigger("StabReady");
        }
        public override void Update()
        {
            if (isReadyToAttack == false && Vector3.Distance(sm.enemy.transform.position, sm.character.transform.position) >= 3f)
            {
                sm.enemy.transform.rotation = Quaternion.Slerp(sm.enemy.transform.rotation, GetLookAtAngle(), Time.deltaTime * 10);
            }
            else
            {
                sm.ChangeState(sm.dashAttackState);
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
            sm.enemy.navMesh.isStopped = false;
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
        IEnumerator GetReady()
        {
            yield return new WaitForSeconds(1f);
            isReadyToAttack = true;
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

    }
}
