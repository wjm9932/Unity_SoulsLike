using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFSM
{
    public class StanbyStabAttackState : EnemyPatternState
    {
        private float timer;
        public StanbyStabAttackState(EnemyBehaviorStateMachine sm) : base(sm)
        {
            stoppingDistance = 0f;
        }

        public override void Enter()
        {
            timer = 1f;
            sm.enemy.navMesh.isStopped = true;
            sm.enemy.animator.SetTrigger("StabReady");
        }
        public override void Update()
        {
            if (timer > 0f && Vector3.Distance(sm.enemy.transform.position, sm.character.transform.position) >= 4f)
            {
                sm.enemy.transform.rotation = Quaternion.Slerp(sm.enemy.transform.rotation, GetLookAtAngle(), Time.deltaTime * 10);
                timer -= Time.deltaTime;
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
